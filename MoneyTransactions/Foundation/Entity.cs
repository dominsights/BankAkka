using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Foundation
{
    public abstract class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is Entity entity &&
                   Id.Equals(entity.Id);
        }

        public abstract override int GetHashCode();
    }
}
