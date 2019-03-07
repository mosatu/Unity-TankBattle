using UnityEngine;

namespace TankBattle
{
    public class TrackData : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private CampType m_OwnerCamp = CampType.Unknown;
        
        public TrackData(int entityId, int typeId, int ownerId, CampType ownerCamp) : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_OwnerCamp = ownerCamp;
            
        }
    }
}