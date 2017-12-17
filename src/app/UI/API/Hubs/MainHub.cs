using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using Clinic.Core.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Api.Hubs
{
    [HubName("AppointmentHub")]
    public class MainHub : Hub
    {

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

        public override Task OnConnected()
        {
            Clients.All.newConnection(Context.ConnectionId);
            return base.OnConnected();
        }
        public void newApp(Appointment content)
        {
            Clients.Others.newApp(content);
        }
        public static void newAppointment(Appointment content)
        {
            //Not used anymore
            hubContext.Clients.All.newAppointment(content);
        }
        public static void updatedAppointment(Appointment content)
        {
            hubContext.Clients.All.updatedAppointment(content);
        }
        public static void eventStatus(Guid id,string status,string patientName)
        {
            dynamic Jobject = new JObject();
            Jobject.id = id;
            Jobject.status = status;
            Jobject.patientName = patientName;
            hubContext.Clients.All.eventStatus(Jobject);
        }
        public static void removed(Guid id)
        {
            hubContext.Clients.All.removed(id);
        }
        public static void paymentReleased(Appointment content)
        {
            hubContext.Clients.All.paymentReleased(content);
        }


    }
}