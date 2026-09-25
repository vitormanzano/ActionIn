using System.Runtime.InteropServices.JavaScript;
using MediatR;

namespace ActionIn.Core.Messages;

public abstract class Event : Message, INotification
{
   public DateTime CreatedOn { get; private set; }

   public Event()
   {
      CreatedOn = DateTime.UtcNow; 
   }
}