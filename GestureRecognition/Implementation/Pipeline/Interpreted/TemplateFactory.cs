using System.Collections.Generic;
using System.IO;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using GestureRecognition.Interface.Commands;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    public static class TemplateFactory
    {
        public static ITemplate CreateTemplate<R, T>() where T : AInterpretedCommand, new() where R : ITemplate, new()
        {
            var gesture = new R();
            gesture.Command = new T {Id = gesture.Id, Name = gesture.DisplayName};
            return gesture;
        }

        public static IEnumerable<ITemplate> CreateTemplates()
        {
            return new List<ITemplate>
            {
                CreateTemplate<ScaleAndRotateGesture, ScaleAndRotate>(),
                CreateTemplate<ExplodeGesture, ExplodeCommand>(),
                CreateTemplate<ImplodeGesture, ImplodeCommand>(),
                CreateTemplate<SamuraiGesture, SamuraiCommand>()
            };
        }
        
        public static IList<ITemplate> TemplatesFromGestureSet(string filename)
        {
            var sr = new StreamReader(filename);
            var templates = new List<ITemplate>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                templates.Add(ParseTemplate(line));
            }
            return templates;
        } 
        
        public static ITemplate ParseTemplate(string templateString)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            //get id in first index, point list in second
            var id_points = templateString.Split('\t');
            var template = new ScaleAndRotateGesture();
            //split list of points into each individual point
            var points = id_points[1].Split(';');
            //go through each string point, extract its components, add point to point list
            var sequence = new List<Vector3>();
            foreach (var p in points)
            {
                if (p != "")
                {
                    var ptComponents = p.Split(',');
                    var pt = new Vector3(double.Parse(ptComponents[0]), double.Parse(ptComponents[1]), double.Parse(ptComponents[2]));
                    sequence.Add(pt);

                    if (int.Parse(ptComponents[0]) < minX)
                    {
                        minX = int.Parse(ptComponents[0]);
                    }
                    if (int.Parse(ptComponents[0]) > maxX)
                    {
                        maxX = int.Parse(ptComponents[0]);
                    }
                }
            }
            template.Add(JointType.HAND_RIGHT, sequence);
            return template;
        }

    }
}
