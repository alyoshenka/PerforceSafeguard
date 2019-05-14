using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

namespace Alexi.Jobs
{
    public class PathfindingJobSuper : MonoBehaviour
    {

        struct PathfindingJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> nodes;

            public void Execute(int i)
            {

            }
        }

    }
}

