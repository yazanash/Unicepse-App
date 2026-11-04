using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.MessengerSystem.Events
{
    public record EntityUpdated<T>(T Entity) where T : class;
    public record PaymentCreated(int SubscriptionId, double AmountPaid, DateTime DatePaid);

    public record PaymentDeleted(int SubscriptionId, double AmountPaid);


    //Bio Engine Events 
    public record StartEnrollmentMessage(Player Player);
    public record StopEnrollmentMessage();
    // إشارات من محرك البصمة إلى باقي النظام
    public record UserFingerprintScannedMessage(int PlayerId, int Score);
    public record EnrollmentStepMessage(int Step, string Status);
    public record EnrollmentCompletedMessage(Player Player, byte[] MasterTemplate);
    public record EnrollmentFailedMessage(string Reason);
}
