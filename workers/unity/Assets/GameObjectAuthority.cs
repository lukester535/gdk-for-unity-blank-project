using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.PlayerLifecycle;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectCreation
{
    public class GameObjectCreatorAuthority : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, GameObject> cachedPrefabs
            = new Dictionary<string, GameObject>();

        private readonly string workerType;
        private readonly Unity.Entities.World workerWorld;
        private readonly Vector3 workerOrigin;

        private readonly Dictionary<EntityId, GameObject> entityIdToGameObject = new Dictionary<EntityId, GameObject>();

        private readonly Type[] componentsToAdd =
        {
            typeof(Transform), typeof(Rigidbody), typeof(MeshRenderer)
        };

        public GameObjectCreatorAuthority(string workerType, Vector3 workerOrigin, ILogDispatcher logger, Unity.Entities.World workerWorld)
        {
            this.workerType = workerType;
            this.workerOrigin = workerOrigin;
            this.workerWorld = workerWorld;
        }

        public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
        {
            entityTypeExpectations.RegisterDefault(new[]
            {
                typeof(Position.Component)
            });
        }

        public void OnEntityCreated(string entityType, SpatialOSEntity entity, EntityGameObjectLinker linker)
        {
            var spatialOSPosition = entity.GetComponent<Position.Component>();

            var prefabName = entityType;
            var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;

            if (!cachedPrefabs.TryGetValue(prefabName, out var prefab))
            {
                bool hasAuthority = PlayerLifecycleHelper.IsOwningWorker(entity.SpatialOSEntityId, this.workerWorld);

                var nonAuth = Path.Combine("Prefabs", workerType, prefabName);
                var auth = Path.Combine("Prefabs", workerType, "Authoritative", prefabName);
                var workerSpecificPath = nonAuth;

                if (hasAuthority)
                {
                    workerSpecificPath = auth;
                }

                var commonPath = Path.Combine("Prefabs", "Common", prefabName);

                prefab = Resources.Load<GameObject>(workerSpecificPath) ?? Resources.Load<GameObject>(commonPath);
                //cachedPrefabs[prefabName] = prefab;
            }

            if (prefab == null)
            {
                return;
            }

            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";

            entityIdToGameObject.Add(entity.SpatialOSEntityId, gameObject);
            linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject, componentsToAdd);
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            if (!entityIdToGameObject.TryGetValue(entityId, out var go))
            {
                return;
            }

            Object.Destroy(go);
            entityIdToGameObject.Remove(entityId);
        }
    }
}
