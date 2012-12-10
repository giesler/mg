namespace System.Data.Services.Client
{
    using System;
    using System.Diagnostics;

    public abstract class Descriptor
    {
        private uint changeOrder = uint.MaxValue;
        private bool saveContentGenerated;
        private Exception saveError;
        private EntityStates saveResultProcessed;
        private EntityStates state;

        internal Descriptor(EntityStates state)
        {
            this.state = state;
        }

        internal uint ChangeOrder
        {
            get
            {
                return this.changeOrder;
            }
            set
            {
                this.changeOrder = value;
            }
        }

        internal bool ContentGeneratedForSave
        {
            get
            {
                return this.saveContentGenerated;
            }
            set
            {
                this.saveContentGenerated = value;
            }
        }

        internal virtual bool IsModified
        {
            get
            {
                Debug.Assert((((EntityStates.Added == this.state) || (EntityStates.Modified == this.state)) || (EntityStates.Unchanged == this.state)) || (EntityStates.Deleted == this.state), "entity state is not valid");
                return (EntityStates.Unchanged != this.state);
            }
        }

        internal abstract bool IsResource { get; }

        internal Exception SaveError
        {
            get
            {
                return this.saveError;
            }
            set
            {
                this.saveError = value;
            }
        }

        internal EntityStates SaveResultWasProcessed
        {
            get
            {
                return this.saveResultProcessed;
            }
            set
            {
                this.saveResultProcessed = value;
            }
        }

        public EntityStates State
        {
            get
            {
                return this.state;
            }
            internal set
            {
                this.state = value;
            }
        }
    }
}

