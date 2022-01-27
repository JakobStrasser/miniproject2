using System;

namespace Miniproject2_JakobStrasser
{
    public class Computer : Asset
    {
        public Computer(string model, DateTime purchaseDate, double uSDPrice, int officeId, int memorySize, int diskSize) : base(model, purchaseDate, uSDPrice, officeId)
        {
            MemorySize = memorySize;
            DiskSize = diskSize;
        }

        public int MemorySize { get; set; }
        public int DiskSize { get; set; }

    }

}
