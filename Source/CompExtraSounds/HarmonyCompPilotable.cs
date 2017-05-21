using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using System.Reflection;
using UnityEngine;

namespace CompPilotable
{
    [StaticConstructorOnStartup]
    static class HarmonyCompPilotable
    {
        static HarmonyCompPilotable()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.jecrell.comps.pilotable");
            harmony.Patch(AccessTools.Method(typeof(DamageWorker_AddInjury), "FinalizeAndAddInjury"), null, new HarmonyMethod(typeof(HarmonyCompPilotable), "FinalizeAndAddInjury_PostFix"));
            harmony.Patch(AccessTools.Method(typeof(Pawn_PathFollower), "StartPath"), new HarmonyMethod(typeof(HarmonyCompPilotable), "StartPath_PreFix"), null);
        }

        // Verse.AI.Pawn_PathFollower
        public static bool StartPath_PreFix(Pawn_PathFollower __instance, LocalTargetInfo dest, PathEndMode peMode)
        {
            Pawn pawn = (Pawn)AccessTools.Field(typeof(Pawn_PathFollower), "pawn").GetValue(__instance);
            if (pawn != null)
            {
                CompPilotable compPilotable = pawn.GetComp<CompPilotable>();
                if (compPilotable != null)
                {
                    if (compPilotable.movingStatus == MovingState.frozen) return false;
                }
            }
            return true;
        }

            // Verse.DamageWorker_AddInjury
            public static void FinalizeAndAddInjury_PostFix(DamageWorker_AddInjury __instance, Pawn pawn, Hediff_Injury injury, DamageInfo dinfo)
        {
            CompPilotable compPilotable = pawn.GetComp<CompPilotable>();
            if (compPilotable != null)
            {
                List<Pawn> affectedPawns = new List<Pawn>();
                
                //Pilot check.
                List<BodyPartRecord> pilotParts = compPilotable.PilotParts;
                if (pilotParts != null && pilotParts.Count > 0)
                {
                    if (pilotParts.Contains(injury.Part))
                    {
                        if (compPilotable.pilots != null && compPilotable.pilots.Count > 0)
                        {
                            affectedPawns.AddRange(compPilotable.pilots);
                        }
                    }
                }
                //Gunner check.
                List<BodyPartRecord> gunnerParts = compPilotable.GunnerParts;
                if (gunnerParts != null && gunnerParts.Count > 0)
                {
                    if (gunnerParts.Contains(injury.Part))
                    {
                        if (compPilotable.gunners != null && compPilotable.gunners.Count > 0)
                        {
                            affectedPawns.AddRange(compPilotable.gunners);
                        }
                    }
                }
                //Crew check.
                List<BodyPartRecord> crewParts = compPilotable.CrewParts;
                if (crewParts != null && crewParts.Count > 0)
                {
                    if (crewParts.Contains(injury.Part))
                    {
                        if (compPilotable.crew != null && compPilotable.crew.Count > 0)
                        {
                            affectedPawns.AddRange(compPilotable.crew);
                        }
                    }
                }
                //Passenger check.
                List<BodyPartRecord> passengerParts = compPilotable.PassengerParts;
                if (passengerParts != null && passengerParts.Count > 0)
                {
                    if (passengerParts.Contains(injury.Part))
                    {
                        if (compPilotable.passengers != null && compPilotable.passengers.Count > 0)
                        {
                            affectedPawns.AddRange(compPilotable.passengers);
                        }
                    }
                }

                //Attack the seatholder
                if (affectedPawns != null && affectedPawns.Count > 0)
                {
                    affectedPawns.RandomElement<Pawn>().TakeDamage(new DamageInfo(dinfo));
                }
            }
        }



    }
}
