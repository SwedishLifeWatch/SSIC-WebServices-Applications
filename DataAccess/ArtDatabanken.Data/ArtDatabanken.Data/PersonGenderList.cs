using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPersonGender interface.
    /// </summary>
    [Serializable]
    public class PersonGenderList : DataId32List<IPersonGender>
    {
        /// <summary>
        /// Get person gender with specified id.
        /// </summary>
        /// <param name='personGenderId'>Id of person gender.</param>
        /// <returns>Requested person gender.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IPersonGender Get(PersonGenderId personGenderId)
        {
            return Get((Int32)personGenderId);
        }
    }
}