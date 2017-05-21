using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CompPilotable
{
    internal class CompProperties_Pilotable : CompProperties
    {
        public SoundDef entrySound = null;
        public SoundDef ejectSound = null;
        public int pilotSeats = 1;
        public int gunnerSeats = 0;
        public int crewSeats = 0;
        public int passengerSeats = 0;
        public int requiredPilots = 1;
        public int requiredGunners = 0;
        public int requiredCrew = 0;
        public float ejectIfBelowHealthPercent = 0.0f;
        public PawnKindDef preferredPilotKind = null;
        public PawnKindDef preferredGunnerKind = null;
        public PawnKindDef preferredCrewKind = null;

        public int TotalCapacity
        {
            get
            {
                return pilotSeats + gunnerSeats + crewSeats + passengerSeats;
            }
        }

        public CompProperties_Pilotable()
        {
            this.compClass = typeof(CompPilotable);
        }
    }
}
