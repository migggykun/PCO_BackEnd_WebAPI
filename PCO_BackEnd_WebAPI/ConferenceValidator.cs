using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public static class ConferenceValidator
    {
        public static bool IsValidSchedule(Conference confrence)
        {
            foreach (var conferenceDay in confrence.ConferenceDays)
            {
                foreach (var activity in conferenceDay.ConferenceActivities)
                {
                    if ((conferenceDay.Start <= activity.ActivitySchedule.Start &&
                       conferenceDay.End >= activity.ActivitySchedule.Start)
                                                                               &&
                       (conferenceDay.Start <= activity.ActivitySchedule.End &&
                       conferenceDay.End >= activity.ActivitySchedule.End))
                    {
                        //Do nothing
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}