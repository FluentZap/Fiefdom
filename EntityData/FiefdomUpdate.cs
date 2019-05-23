using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fiefdom.Context;

namespace Fiefdom
{
	public static class FiefdomUpdate
	{
		public static HashSet<string> ConnectedUsers = new HashSet<string>();
		public static void UpdateResources(Fief fief)
		{
			foreach (FiefdomPlot plot in fief.FiefdomPlot)
			{
				switch (plot.Type)
				{
					case "Inn":
						fief.FiefdomResources.Where(r => r.Type == "Gold").FirstOrDefault().Quantity += 50;
						break;

					case "Gold":
						fief.FiefdomResources.Where(r => r.Type == "Wood").FirstOrDefault().Quantity += 3;
						fief.FiefdomResources.Where(r => r.Type == "Food").FirstOrDefault().Quantity += 3;
						fief.FiefdomResources.Where(r => r.Type == "Stone").FirstOrDefault().Quantity += 3;
						break;

						case "Barracks":
						var stone = fief.FiefdomResources.Where(r => r.Type == "Stone").FirstOrDefault();
						var wood = fief.FiefdomResources.Where(r => r.Type == "Wood").FirstOrDefault();
						if (stone.Quantity >= 5 && wood.Quantity >= 5)
						{
							fief.FiefdomResources.Where(r => r.Type == "Gold").FirstOrDefault().Quantity += 1000;
							stone.Quantity -= 5;
							wood.Quantity -= 5;
						}
						break;

					case "WoodCutter":
						fief.FiefdomResources.Where(r => r.Type == "Wood").FirstOrDefault().Quantity += 10;
						break;
					case "Farm":
						fief.FiefdomResources.Where(r => r.Type == "Food").FirstOrDefault().Quantity += 10;
						break;
					case "Quarry":
						fief.FiefdomResources.Where(r => r.Type == "Stone").FirstOrDefault().Quantity += 10;
						break;
					default:
						break;
				}
			}
		}
	}
}
