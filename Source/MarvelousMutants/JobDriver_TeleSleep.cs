using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace MarvelousMutants
{
    public class JobDriver_TeleSleep : JobDriver
    {
        private int timeToSleep;
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(TargetA, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            yield return new Toil
            {
                initAction = () =>
                {
                    pawn.pather.StopDead();
                    pawn.jobs.posture = PawnPosture.LayingOnGroundNormal;
                    pawn.GetComp<CompCanBeDormant>()?.ToSleep();
                    timeToSleep = job.expiryInterval;
                },
                tickAction = () =>
                {
                    timeToSleep--;
                    if (timeToSleep <= 0)
                    {
                        pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = job.expiryInterval
            };
        }
    }
}