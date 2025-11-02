using System;

namespace Assets.Source.Scripts.Utility
{
    public interface IResource
    {
        public event Action ResourcesAmountChanged;
        public event Action ResourceOver;

        public int Amount { get; }

        public int Maximum { get; }

        public float Percent => (float) Amount / Maximum;
    }
}