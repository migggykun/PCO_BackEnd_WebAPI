using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public static class ConferenceValidator
    {
        private static string[] errorMessage =
        {
        "Invalid date in conference day. Must be within conference date range",
        "Invalid time schedule in conference acitivity",
        "Invalid date. Conference day's date is already taken!",
        "Cannot update multiple conferences at the same time."
        };
        public static bool IsValidSchedule(Conference conference, out string message, int id = -1)
        {
            message = string.Empty;

            for (int i = 0; i < conference.ConferenceDays.Count; i++)
            {
                for (int ii = 1+i; ii < conference.ConferenceDays.Count; ii++)
                {
                    if (conference.ConferenceDays.ToList()[i].Date == conference.ConferenceDays.ToList()[ii].Date)
                    {
                        message = errorMessage[2];
                        return false;
                    }
                }
            }

            foreach (var conferenceDay in conference.ConferenceDays)
            {
                if(id != -1 && id != conferenceDay.ConferenceId)
                {
                    message = errorMessage[3];
                    return false;
                }

                if ((conference.Start <= conferenceDay.Date &&
                   conference.End >= conferenceDay.Date))
                {
                    //Do nothing
                }
                else
                {
                    message = errorMessage[0];
                    return false;
                }

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
                        message = errorMessage[1];
                        return false;
                    }
                }
            }
            return true;
        }
    }
}