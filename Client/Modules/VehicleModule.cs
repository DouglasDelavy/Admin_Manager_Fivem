using System;
using System.Reflection.Emit;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace DevTools.Client.Modules
{
    public class VehicleModule
    {
        #region Init
        protected MainClient Client { get; }
        internal static VehicleModule Instance { get; private set; }

        internal static void Init(MainClient client)
        {
            Instance ??= new VehicleModule(client);
        }

        #endregion

        public VehicleModule(MainClient client)
        {
            Client = client;

            Client.AddCommand("vehicle", VehicleCommand);
            Client.AddCommand("dv", RemoveVehicleCommand);
        }

        private void RemoveVehicleCommand(string[] args)
        {
            var playerPed = Game.PlayerPed;
            var isClosestVehicle = GetClosestVehicle(playerPed.Position, out var vehicle);
            
            if (isClosestVehicle)
                RemoveEntity(vehicle);
        }

        private async void VehicleCommand(string[] args)
        {
            var playerPed = Game.PlayerPed;
            var playerCurrentVehicle = playerPed.CurrentVehicle;

            if (args.Length < 1)
                return;

            if (playerCurrentVehicle != null)
                RemoveEntity(playerCurrentVehicle);

            var vehicleModelHash = API.GetHashKey(args[0]);
            var currentVehicle = await CreateVehicle(new Model(vehicleModelHash), playerPed.Position, playerPed.Heading, Client.GetCurrentResourceName());

            playerPed.Task.WarpIntoVehicle(currentVehicle, VehicleSeat.Driver);
            Logger.LogDebug($"Client spawn new Vehicle: {currentVehicle.DisplayName} Handle: {currentVehicle.Handle} NetworkId: {currentVehicle.NetworkId}");
        }

        private void RemoveEntity(Entity entity, bool isNetwork = false)
        {
            //TODO: implement Delete Vehicle if is a Network vehicle
            Logger.LogInformation($"Vehicle is being deleted! Handle: {entity.Handle}");

            entity.IsPersistent = true;
            entity.Delete();
        }

        private async Task<Vehicle> CreateVehicle(Model model, Vector3 position, float heading, string plate)
        {
            var vehicle = await World.CreateVehicle(model, position, heading);
            if (vehicle == null) return null;

            vehicle.LockStatus = VehicleLockStatus.Unlocked;
            vehicle.Mods.LicensePlate = plate;
            vehicle.IsEngineRunning = true;
            vehicle.NeedsToBeHotwired = false;

            return vehicle;
        }

        private bool GetClosestVehicle(Vector3 position, out Vehicle vehicle, float distance = 7f)
        {
            foreach (int vehicleHandle in API.GetGamePool("CVehicle"))
            {
                var currentVehicle = (Vehicle)Entity.FromHandle(vehicleHandle);
                if (!(World.GetDistance(position, currentVehicle.Position) <= distance)) continue;
                
                vehicle = currentVehicle;
                return true;
            }

            vehicle = null;
            return false;
        }
    }
}
