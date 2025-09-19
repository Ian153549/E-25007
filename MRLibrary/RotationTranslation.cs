using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class RotationTranslation
    {
        public PointD ObjectLocation;

        public double Rotation;

        public PointD Translation;

        public RotationTranslation()
        {
            ObjectLocation = new PointD();
            Translation = new PointD();
        }

        public RotationTranslation(RotationTranslation rt)
        {
            ObjectLocation = new PointD(rt.ObjectLocation);
            Rotation = rt.Rotation;
            Translation = new PointD(rt.Translation);
        }

        public RotationTranslation(PointD objLocation, double rotation, PointD translation)
        {
            ObjectLocation = objLocation;
            Rotation = rotation;
            Translation = translation;
        }

        public static RotationTranslation operator +(RotationTranslation a, RotationTranslation b)
        {
            RotationTranslation rotationTranslation = new RotationTranslation()
            {
                Rotation = a.Rotation + b.Rotation,
                Translation = a.Translation + b.Translation
            };
            return rotationTranslation;
        }

        public static RotationTranslation operator -(RotationTranslation a, RotationTranslation b)
        {
            RotationTranslation rotationTranslation = new RotationTranslation()
            {
                Rotation = a.Rotation - b.Rotation,
                Translation = a.Translation - b.Translation
            };
            return rotationTranslation;
        }
    }
}
