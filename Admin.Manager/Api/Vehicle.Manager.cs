using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Admin.Manager.Api
{
    public class VehicleApi : BaseScript
    {
        public async Task<int> SpawnVehicle(string model, Vector4 position, string plate, double fuel = 100, bool locked = true)
        {
            uint modelHash = (uint)GetHashKey(model);
            RequestModel(modelHash);
            while (!HasModelLoaded(modelHash))
            {
                await Delay(50);
            }
            var vehicle = CreateVehicle((uint)modelHash, position.X, position.Y, position.Z, position.W, true, true);

            SetVehicleIsStolen(vehicle, false);
            SetVehicleOnGroundProperly(vehicle);
            SetEntityInvincible(vehicle, false);

            SetVehicleNumberPlateText(vehicle, plate);
            SetEntityAsMissionEntity(vehicle, true, true);
            SetVehicleHasBeenOwnedByPlayer(vehicle, true);

            SetVehicleDirtLevel(vehicle, 0f);

            SetVehRadioStation(vehicle, "OFF");
            SetModelAsNoLongerNeeded(modelHash);

            return vehicle;
        }

        public void DeleteVehicleAsync(int vehicle)
        {
            if (!IsEntityAVehicle(vehicle))
                return;

            SetVehicleHasBeenOwnedByPlayer(vehicle, false);
            SetEntityAsMissionEntity(vehicle, true, true);
            DeleteVehicle(ref vehicle);
        }

        public int GetVehicle(float radius)
        {
            var playerPed = PlayerPedId();
            var pedPosition = GetEntityCoords(playerPed, false);

            if (IsPedSittingInAnyVehicle(playerPed))
            {
                return GetVehiclePedIsIn(playerPed, false);
            }
            else
            {
                var vehicle = GetClosestVehicle(pedPosition.X + 0.0001f, pedPosition.Y + 0.0001f, pedPosition.Z + 0.0001f, radius + 0.0001f, 0, 8192 + 4096 + 4 + 2 + 1);
                if (!IsEntityAVehicle(vehicle))
                    vehicle = GetClosestVehicle(pedPosition.X + 0.0001f, pedPosition.Y + 0.0001f, pedPosition.Z + 0.0001f, radius + 0.0001f, 0, 4 + 2 + 1);

                return vehicle;
            }
        }

        public void FixVehicleAsync(int vehicle)
        {
            if (!IsEntityAVehicle(vehicle))
                return;

            SetVehicleFixed(vehicle);
            SetVehicleFuelLevel(vehicle, 100.0f);
            SetVehicleOnGroundProperly(vehicle);
            SetVehicleUndriveable(vehicle, false);
            SetEntityAsMissionEntity(vehicle, true, true);
        }

        public void TunningVehicleAsync(int vehicle)
        {
            if (!IsEntityAVehicle(vehicle))
                return;

            SetVehicleModKit(vehicle, 0);
            SetVehicleMod(vehicle, 11, GetNumVehicleMods(vehicle, 11) - 1, false);
            SetVehicleMod(vehicle, 12, GetNumVehicleMods(vehicle, 12) - 1, false);
            SetVehicleMod(vehicle, 13, GetNumVehicleMods(vehicle, 13) - 1, false);
            SetVehicleMod(vehicle, 15, GetNumVehicleMods(vehicle, 15) - 2, false);
            SetVehicleMod(vehicle, 16, GetNumVehicleMods(vehicle, 16) - 1, false);
            ToggleVehicleMod(vehicle, 18, true);
            SetVehicleTyresCanBurst(vehicle, false);
        }
    }
}
