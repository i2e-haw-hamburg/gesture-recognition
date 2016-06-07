using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using Leap;

namespace LeapRecorder
{
    public class LeapFrameSerializer : ILeapSerializer
    {
        public byte[] Serialize(Leap.Frame frame)
        {
            var f = new SerializableFrame
            {
                id = frame.Id,
                timestamp = frame.Timestamp,
                fps = frame.CurrentFramesPerSecond,
                hands = frame.Hands.Select(ToSerializableHand).ToList()
            };
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, f);
                return ms.ToArray();
            }
        }

        public Frame Deserialize(byte[] bytes)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var frame = (SerializableFrame)binForm.Deserialize(memStream);
            return new Frame(frame.id, frame.timestamp, frame.fps, null, frame.hands.Select(ToHand).ToList());
        }

        private static Hand ToHand(SerializableHand hand)
        {
            return new Hand(hand.frameID, hand.id, hand.confidence, hand.grabStrength, hand.grabAngle, hand.pinchStrength, hand.pinchDistance, hand.palmWidth,
                hand.isLeft, hand.timeVisible, ToArm(hand.arm), hand.fingers.Select(ToFinger).ToList(), hand.palmPosition.ToVector(), hand.stabilizedPalmPosition.ToVector(),
                hand.palmVelocity.ToVector(), hand.palmNormal.ToVector(), hand.direction.ToVector(), hand.wristPosition.ToVector());
        }

        private static Finger ToFinger(SerializableFinger finger)
        {
            return new Finger(finger.frameID, finger.handId, finger.fingerId, finger.timeVisible, finger.tipPosition.ToVector(), finger.tipVelocity.ToVector(), finger.direction.ToVector(),
                finger.stabilizedTipPosition.ToVector(), finger.width, finger.length, finger.isExtended, finger.type, ToBone(finger.metacarpal), ToBone(finger.proximal), ToBone(finger.intermediate),
                ToBone(finger.distal));
        }

        private static Bone ToBone(SerializableBone bone)
        {
            return new Bone(bone.prevJoint.ToVector(), bone.nextJoint.ToVector(), bone.center.ToVector(), bone.direction.ToVector(), bone.length, bone.width, bone.type, bone.rotation.ToLeapQuaternion());
        }

        private static Arm ToArm(SerializableArm arm)
        {
            return new Arm(arm.elbow.ToVector(), arm.wrist.ToVector(), arm.center.ToVector(), arm.direction.ToVector(), arm.length, arm.width, arm.rotation.ToLeapQuaternion());
        }

        private static SerializableHand ToSerializableHand(Hand leapHand)
        {
            var hand = new SerializableHand
            {
                id = leapHand.Id,
                frameID = leapHand.FrameId,
                confidence = leapHand.Confidence,
                grabStrength = leapHand.GrabStrength,
                grabAngle = leapHand.GrabAngle,
                pinchStrength = leapHand.PinchStrength,
                pinchDistance = leapHand.PinchDistance,
                palmWidth = leapHand.PalmWidth,
                isLeft = leapHand.IsLeft,
                timeVisible = leapHand.TimeVisible,
                arm = ToSerializableArm(leapHand.Arm),
                fingers = leapHand.Fingers.Select(ToSerializableFinger).ToList(),
                palmPosition = ToSerializableVector(leapHand.PalmPosition),
                stabilizedPalmPosition = ToSerializableVector(leapHand.StabilizedPalmPosition),
                palmVelocity = ToSerializableVector(leapHand.PalmVelocity),
                palmNormal = ToSerializableVector(leapHand.PalmNormal),
                direction = ToSerializableVector(leapHand.Direction),
                wristPosition = ToSerializableVector(leapHand.WristPosition)
            };
            return hand;
        }

        private static SerializableArm ToSerializableArm(Arm leapArm)
        {
            return new SerializableArm
            {
                elbow = ToSerializableVector(leapArm.ElbowPosition),
                wrist = ToSerializableVector(leapArm.WristPosition),
                center = ToSerializableVector(leapArm.Center),
                direction = ToSerializableVector(leapArm.Direction),
                length = leapArm.Length,
                width = leapArm.Width,
                rotation = ToSerializableQuaternion(leapArm.Rotation)
            };
        }

        private static SerializableFinger ToSerializableFinger(Finger leapFinger)
        {
            return new SerializableFinger
            {
                handId = leapFinger.HandId,
                fingerId = leapFinger.Id,
                timeVisible = leapFinger.TimeVisible,
                tipPosition = ToSerializableVector(leapFinger.TipPosition),
                tipVelocity = ToSerializableVector(leapFinger.TipVelocity),
                direction = ToSerializableVector(leapFinger.Direction),
                stabilizedTipPosition = ToSerializableVector(leapFinger.StabilizedTipPosition),
                width = leapFinger.Width,
                length = leapFinger.Length,
                isExtended = leapFinger.IsExtended,
                type = leapFinger.Type,
                metacarpal = ToSerializableBone(leapFinger.Bone(Bone.BoneType.TYPE_METACARPAL)),
                proximal = ToSerializableBone(leapFinger.Bone(Bone.BoneType.TYPE_PROXIMAL)),
                intermediate = ToSerializableBone(leapFinger.Bone(Bone.BoneType.TYPE_INTERMEDIATE)),
                distal = ToSerializableBone(leapFinger.Bone(Bone.BoneType.TYPE_DISTAL)),
            };
        }

        private static SerializableBone ToSerializableBone(Bone bone)
        {
            return new SerializableBone
            {
                prevJoint = ToSerializableVector(bone.PrevJoint),
                nextJoint = ToSerializableVector(bone.NextJoint),
                center = ToSerializableVector(bone.Center),
                direction = ToSerializableVector(bone.Direction),
                width = bone.Width,
                length = bone.Length,
                type = bone.Type,
                rotation = ToSerializableQuaternion(bone.Rotation)
            };
        }

        private static SerializableQuaternion ToSerializableQuaternion(LeapQuaternion quaternion)
        {
            return new SerializableQuaternion
            {
                x = quaternion.x,
                y = quaternion.y,
                z = quaternion.z,
                w = quaternion.w
            };
        }

        private static SerializableVector ToSerializableVector(Vector vector)
        {
            return new SerializableVector
            {
                x = vector.x,
                y = vector.y,
                z = vector.z
            };
        }
    }

    [Serializable]
    public class SerializableFrame
    {
        public long id;
        public long timestamp;
        public float fps;
        public List<SerializableHand> hands;
    }

    [Serializable]
    public class SerializableHand
    {
        public long frameID;
        public int id;
        public float confidence;
        public float grabStrength;
        public float grabAngle;
        public float pinchStrength;
        public float pinchDistance;
        public float palmWidth;
        public bool isLeft;
        public float timeVisible;
        public SerializableArm arm;
        public List<SerializableFinger> fingers;
        public SerializableVector palmPosition;
        public SerializableVector stabilizedPalmPosition;
        public SerializableVector palmVelocity;
        public SerializableVector palmNormal;
        public SerializableVector direction;
        public SerializableVector wristPosition;
    }

    [Serializable]
    public class SerializableFinger
    {
        public long frameID;
        public int handId;
        public int fingerId;
        public float timeVisible;
        public SerializableVector tipPosition;
        public SerializableVector tipVelocity;
        public SerializableVector direction;
        public SerializableVector stabilizedTipPosition;
        public float width;
        public float length;
        public bool isExtended;
        public Finger.FingerType type;
        public SerializableBone metacarpal;
        public SerializableBone proximal;
        public SerializableBone intermediate;
        public SerializableBone distal;
    }

    [Serializable]
    public class SerializableBone
    {
        public SerializableVector prevJoint;
        public SerializableVector nextJoint;
        public SerializableVector center;
        public SerializableVector direction;
        public float width;
        public float length;
        public Bone.BoneType type;
        public SerializableQuaternion rotation;
    }

    [Serializable]
    public class SerializableArm
    {
        public SerializableVector elbow;
        public SerializableVector wrist;
        public SerializableVector center;
        public SerializableVector direction;
        public float length;
        public float width;
        public SerializableQuaternion rotation;
    }

    [Serializable]
    public class SerializableVector
    {
        public float x;
        public float y;
        public float z;

        public Vector ToVector() => new Vector(x, y, z);
    }

    [Serializable]
    public class SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public LeapQuaternion ToLeapQuaternion() => new LeapQuaternion(x, y, z, w);
    }
}