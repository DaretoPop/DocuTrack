using System;
using System.Collections.Generic;
using System.Text;

namespace DocuTrack.UI.Scaling
{
    /// <summary>
    /// Represents a context that can be scaled by the <see cref="ScalingManager"/>.
    /// </summary>
    public class BindingContext : HashSet<ScalableObject>
    {
    }
}
