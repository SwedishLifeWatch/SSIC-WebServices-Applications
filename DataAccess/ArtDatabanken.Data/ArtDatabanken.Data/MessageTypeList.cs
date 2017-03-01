using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IMessageType interface.
    /// </summary>
    [Serializable]
    public class MessageTypeList : DataId32List<IMessageType>
    {
        /// <summary>
        /// Get Message type with specified id.
        /// </summary>
        /// <param name='messageTypeId'>Id of Message type.</param>
        /// <returns>Requested Message type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IMessageType Get(MessageTypeId messageTypeId)
        {
            return Get((Int32)messageTypeId);
        }

        /// <summary>
        /// Returns a clone of this list
        /// </summary>
        public MessageTypeList CloneMessageTypeList ()
        {
            MessageTypeList messageTypeListClone = new MessageTypeList();
            // messageTypeListClone = (MessageTypeList) this.MemberwiseClone();
            foreach (IMessageType messageType in this)
            {
                IMessageType cloneMessageType = new MessageType(messageType.Id, 
                                                                messageType.Name, 
                                                                messageType.NameStringId, 
                                                                messageType.DataContext);
                messageTypeListClone.Add(cloneMessageType);
            }
            return messageTypeListClone;
        }
    }
}
