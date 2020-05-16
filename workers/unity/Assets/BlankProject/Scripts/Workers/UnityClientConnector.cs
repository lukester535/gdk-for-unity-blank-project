using System;
using BlankProject.Scripts.Config;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Worker.CInterop;
using UnityEngine;
using System.Diagnostics;
using System.Security.Cryptography;
using Improbable;
using Improbable.Gdk.Subscriptions;

namespace BlankProject
{
    public class UnityClientConnector : WorkerConnector
    {
        public const string WorkerType = "UnityClient";

        private async void Start()
        {

            var connParams = CreateConnectionParameters(WorkerType);

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType)));
            }

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
                PlayerLifecycleHelper.AddClientSystems(Worker.World);
                
                GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, new GameObjectCreatorAuthority(Worker.WorkerType, Worker.Origin, Worker.LogDispatcher, Worker.World));
        }



    }
}
