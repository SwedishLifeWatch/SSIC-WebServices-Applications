using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details
{
    /// <summary>
    /// Observation view model
    /// </summary>
    public class ObservationDetailViewModel
    {
        public List<ObservationDetailFieldViewModel> Fields { get; set; }

        public List<ProjectViewModel> Projects { get; set; }

        public string ObservationId { get; set; }
        public bool ShowAsDialog { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }
    }

    /// <summary>
    /// Project view model.
    /// </summary>
    public class ProjectViewModel
    {
        /// <summary>
        /// Project Id.
        /// </summary>        
        public int Id { get; set; }
        
        /// <summary>
        /// Project Guid.
        /// </summary>        
        public string Guid { get; set; }
        
        /// <summary>
        /// Project name.
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// Project parameters grouped by PropertyIdentifier.
        /// </summary>        
        [JsonIgnore]
        public Dictionary<string, ProjectParameterObservationDetailFieldViewModel> ProjectParameters { get; set; }

        [JsonProperty(PropertyName = "ProjectParameters")]
        public List<ProjectParameterObservationDetailFieldViewModel> ProjectParameterList
        {
            get { return ProjectParameters.Values.ToList(); }
        }

        /// <summary>
        /// Determines whether the specified <see cref="ProjectViewModel" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ProjectViewModel" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="ProjectViewModel" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(ProjectViewModel other)
        {
            return Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectViewModel) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id;
        }

        public ProjectViewModel Clone()
        {
            ProjectViewModel other = (ProjectViewModel) this.MemberwiseClone();
            other.ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>(this.ProjectParameters.Count);
            foreach (var pair in this.ProjectParameters)
            {
                other.ProjectParameters.Add(pair.Key, pair.Value.Clone());
            }
            
            return other;            
        }        

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Id: {0}, Name: {1}", Id, Name);
        }
    }
}