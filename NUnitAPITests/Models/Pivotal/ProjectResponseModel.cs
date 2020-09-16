using System;
using Newtonsoft.Json;

namespace NUnitAPITests.Models.Pivotal
{
    public class ProjectResponseModel
    {
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("iteration_length")] public int IterationLength { get; set; }

        [JsonProperty("week_start_day")] public string WeekStartDay { get; set; }

        [JsonProperty("point_scale")] public string PointScale { get; set; }

        [JsonProperty("point_scale_is_custom")]
        public bool PointScaleIsCustom { get; set; }

        [JsonProperty("bugs_and_chores_are_estimatable")]
        public bool BugsAndChoresAreEstimatable { get; set; }

        [JsonProperty("automatic_planning")] public bool AutomaticPlanning { get; set; }

        [JsonProperty("enable_tasks")] public bool EnableTasks { get; set; }

        [JsonProperty("time_zone")] public TimeZone TimeZone { get; set; }

        [JsonProperty("velocity_averaged_over")]
        public int VelocityAveragedOver { get; set; }

        [JsonProperty("number_of_done_iterations_to_show")]
        public int NumberOfDoneIterationsToShow { get; set; }

        [JsonProperty("has_google_domain")] public bool HasGoogleDomain { get; set; }

        [JsonProperty("enable_incoming_emails")]
        public bool EnableIncomingEmails { get; set; }

        [JsonProperty("initial_velocity")] public int InitialVelocity { get; set; }

        [JsonProperty("public")] public bool Public { get; set; }

        [JsonProperty("atom_enabled")] public bool AtomEnabled { get; set; }

        [JsonProperty("project_type")] public string ProjectType { get; set; }

        [JsonProperty("start_time")] public string StartTime { get; set; }

        [JsonProperty("created_at")] public string CreatedAt { get; set; }

        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }

        [JsonProperty("account_id")] public int AccountId { get; set; }

        [JsonProperty("current_iteration_number")]
        public int CurrentIterationNumber { get; set; }

        [JsonProperty("enable_following")] public bool EnableFollowing { get; set; }
    }

    public class TimeZone
    {
        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("olson_name")] public string OlsonName { get; set; }

        [JsonProperty("offset")] public string Offset { get; set; }
    }
}