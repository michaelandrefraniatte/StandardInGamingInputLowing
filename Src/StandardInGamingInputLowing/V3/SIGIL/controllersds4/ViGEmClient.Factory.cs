﻿using System.Diagnostics.CodeAnalysis;

namespace controllersds4
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed partial class ViGEmClient
    {

        /// <summary>
        ///     Allocates a new virtual DualShock 4 device on the bus.
        /// </summary>
        /// <returns>A new virtual DualShock 4 Controller device.</returns>
        public IDualShock4Controller CreateDualShock4Controller()
        {
            return new DualShock4Controller(this);
        }

        /// <summary>
        ///     Allocates a new virtual DualShock 4 device on the bus.
        /// </summary>
        /// <param name="vendorId">16-bit unsigned vendor ID.</param>
        /// <param name="productId">16-bit unsigned product ID.</param>
        /// <returns>A new virtual DualShock 4 Controller device.</returns>
        public IDualShock4Controller CreateDualShock4Controller(ushort vendorId, ushort productId)
        {
            return new DualShock4Controller(this, vendorId, productId);
        }
    }
}