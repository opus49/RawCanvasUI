﻿using System.Collections.Generic;
using RawCanvasUI.Interfaces;

namespace RawCanvasUI.Util
{
    /// <summary>
    /// Finds controls within a container.
    /// </summary>
    internal static class ControlFinder
    {
        /// <summary>
        /// Finds any controls nested within the container.
        /// </summary>
        /// <param name="container">The container to search.</param>
        /// <returns>An enumerable of controls.</returns>
        public static IEnumerable<IControl> FindControls(IContainer container)
        {
            foreach (var item in container.Items)
            {
                if (item is IControl control && control.IsEnabled)
                {
                    yield return control;
                }

                if (item is IContainer childContainer)
                {
                    foreach (var childControl in FindControls(childContainer))
                    {
                        yield return childControl;
                    }
                }
            }
        }
    }
}
