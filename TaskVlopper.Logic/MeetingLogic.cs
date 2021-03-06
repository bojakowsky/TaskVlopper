﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskVlopper.Base.Logic;
using TaskVlopper.Base.Model;
using TaskVlopper.Base.Repository;

namespace TaskVlopper.Logic
{
    public class MeetingLogic : IMeetingLogic
    {
        private readonly IMeetingRepository MeetingRepository;
        private readonly IMeetingParticipantsRepository MeetingParticipantsRepository;

        public MeetingLogic(IMeetingRepository meetingRepository, IMeetingParticipantsRepository meetingParticipantsRepository)
        {
            MeetingRepository = meetingRepository;
            MeetingParticipantsRepository = meetingParticipantsRepository;
        }

        public IEnumerable<Meeting> GetAllMeetingsForCurrentUser(string userId)
        {
            return MeetingParticipantsRepository.GetMeetingParticipantsByUserId(userId)
                    .Select(meetingParticipants =>
                        MeetingRepository.GetMeetingByIdWithoutTrackingQueryable(meetingParticipants.MeetingID).Single())
                    .ToList();
        }

        public IEnumerable<Meeting> GetAllMeetingsForCurrentUserAndProject(string userId, int projectId)
        {
            return MeetingRepository.GetMeetingByProjectId(projectId)
                .Where(meetingByProject =>
                    MeetingParticipantsRepository.GetMeetingParticipantsByUserId(userId)
                        .Any(meetingParticipants => meetingParticipants.MeetingID == meetingByProject.ID)
                    )
                .ToList();
        }

        public IEnumerable<Meeting> GetAllMeetingsForCurrentUserAndProjectAndTask(string userId, int projectId, int taskId)
        {
            return MeetingRepository.GetMeetingByProjectIdAndTaskId(projectId, taskId)
                .Where(meetingByProject =>
                    MeetingParticipantsRepository.GetMeetingParticipantsByUserId(userId)
                        .Any(meetingParticipants => meetingParticipants.MeetingID == meetingByProject.ID)
                    )
                .ToList();
        }

        public void HandleMeetingAdd(Meeting meeting, int projectId, int? taskId, string userId)
        {
            meeting.ProjectID = projectId;
            meeting.TaskID = taskId;
            MeetingRepository.Add(meeting);

            MeetingParticipants participant = new MeetingParticipants();
            participant.MeetingID = meeting.ID;
            participant.UserID = userId;
            MeetingParticipantsRepository.Add(participant);
        }

        public void HandleMeetingDelete(int projectId, int? taskId, int id, string userId)
        {
            var meeting = MeetingRepository.GetMeetingByIdWithTracking(id);
            MeetingRepository.Remove(meeting);

            var meetingParticipants = 
                MeetingParticipantsRepository.GetMeetingParticipantsByMeetingId(id);

            MeetingParticipantsRepository.RemoveMany(meetingParticipants);

            
        }

        public void HandleMeetingEdit(Meeting meeting, int projectId, int? taskId, int id)
        {
            meeting.ID = id;
            meeting.ProjectID = projectId;
            if (taskId != null) meeting.TaskID = taskId;
            MeetingRepository.Update(meeting);
        }

        public Meeting HandleMeetingGet(int projectId, int? taskId, int id)
        {
            if (taskId != null)
                return MeetingRepository.GetMeetingByIdWithoutTrackingQueryable(id).Where(x => x.ProjectID == projectId && x.TaskID == taskId).Single();
            else
                return MeetingRepository.GetMeetingByIdWithoutTrackingQueryable(id).Where(x => x.ProjectID == projectId).Single();
        }

        public IQueryable<Meeting> HandleMeetingGetQueryable(int projectId, int? taskId, int id)
        {
            if (taskId != null)
                return MeetingRepository.GetMeetingByIdWithoutTrackingQueryable(id).Where(x => x.ProjectID == projectId && x.TaskID == taskId);
            else
                return MeetingRepository.GetMeetingByIdWithoutTrackingQueryable(id).Where(x => x.ProjectID == projectId);
        }

        public void AssignUserToMeeting(int meetingId, string userId)
        {
            if (!MeetingParticipantsRepository.GetMeetingParticipantsByMeetingId(meetingId).Any(x => x.UserID == userId))
            {
                MeetingParticipants participant = new MeetingParticipants();
                participant.MeetingID = meetingId;
                participant.UserID = userId;

                MeetingParticipantsRepository.Add(participant);
            }
        }

        public void UnassignUserFromMeeting(int meetingId, string userId)
        {
            var model = MeetingParticipantsRepository.GetMeetingParticipantsByMeetingId(meetingId)
                .Where(x => x.UserID == userId);
            if (model.Any())
            {
                MeetingParticipantsRepository.Remove(model.Single());
            }
        }

        public IEnumerable<string> GetAllUsersForGivenMeeting(int meetingId)
        {
            return MeetingParticipantsRepository.GetAllUsersIDsByMeeting(meetingId);
        }

        public int CountAllUsersForMeeting(int meetingId)
        {
            return GetAllUsersForGivenMeeting(meetingId).Count();
        }

        public int CountAllMeetingsForCurrentUser(string userId)
        {
            return GetAllMeetingsForCurrentUser(userId).Count();
        }

        public int CountAllMeetingsForCurrentUserAndProject(string userId, int projectId)
        {
            return GetAllMeetingsForCurrentUserAndProject(userId, projectId).Count();
        }

        public int CountAllMeetingsForCurrentUserAndProjectAndTask(string userId, int projectId, int taskId)
        {
            return GetAllMeetingsForCurrentUserAndProjectAndTask(userId, projectId, taskId).Count();
        }

        public int CountAllFutureMeetingsForCurrentUser(string userId)
        {
            return GetAllMeetingsForCurrentUser(userId).Where(x => x.DateAndTime > DateTime.Now).Count();
        }

        public int CountAllFutureMeetingsForCurrentUserAndProject(string userId, int projectId)
        {
            return GetAllMeetingsForCurrentUserAndProject(userId, projectId).Where(x => x.DateAndTime > DateTime.Now).Count();
        }

        public int CountAllFutureMeetingsForCurrentUserAndProjectAndTask(string userId, int projectId, int taskId)
        {
            return GetAllMeetingsForCurrentUserAndProjectAndTask(userId, projectId, taskId).Where(x => x.DateAndTime > DateTime.Now).Count();
        }
    }
}
