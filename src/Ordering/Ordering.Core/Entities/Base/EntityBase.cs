using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        public virtual TId Id { get; protected set; }

        int? _requestedHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not EntityBase<TId>)
                return false;
            else if (ReferenceEquals(this, obj))
                return true;

            var item = (EntityBase<TId>)obj;
            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item == this;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; //XOR for random distribution
                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            return left.Equals(right);
        }

        public static bool operator != (EntityBase<TId> left, EntityBase<TId> right)
        {
            return !(left == right);
        }
    }
}
