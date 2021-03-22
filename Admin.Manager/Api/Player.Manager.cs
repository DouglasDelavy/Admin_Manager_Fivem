using CitizenFX.Core;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Admin.Manager.Api
{
    public class PlayerApi : BaseScript
    {
		public async void TeleportWayPoint()
		{
			var markerId = GetFirstBlipInfoId(8);

			if (DoesBlipExist(markerId))
			{
				float defaultValue = 0.0f;
				var markerCoords = GetBlipInfoIdCoord(markerId);
				Convert.ToSingle(markerCoords.X);
				Convert.ToSingle(markerCoords.Y);
				Convert.ToSingle(markerCoords.Z);

				for (int i = 1; i < 999; i++)
				{
					SetPedCoordsKeepVehicle(PlayerPedId(), markerCoords.X, markerCoords.Y, i);
					bool groundZ = GetGroundZFor_3dCoord(markerCoords.X, markerCoords.Y, i, ref defaultValue, false);

					if (groundZ)
					{
						SetPedCoordsKeepVehicle(PlayerPedId(), markerCoords.X, markerCoords.Y, i + 0.3f);
						break;
					}
					await Delay(10);
				}
			}
			else
			{
				Screen.ShowNotification("~b~There is no marker to teleport~b~");
			}
		}
	}
}
