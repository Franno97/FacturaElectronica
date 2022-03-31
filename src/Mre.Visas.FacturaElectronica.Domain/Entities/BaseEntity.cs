using System;

namespace Mre.Visas.FacturaElectronica.Domain.Entities
{
    public class BaseEntity
    {
        #region Constructors

        public BaseEntity()
        {
        }

        #endregion Constructors

        #region Properties

        public Guid Id { get; protected set; }

        public bool IsDeleted { get; set; }

        #endregion Properties

        #region Methods

        public void AssignId()
        {
            Id = Guid.NewGuid();
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        #endregion Methods
    }
}