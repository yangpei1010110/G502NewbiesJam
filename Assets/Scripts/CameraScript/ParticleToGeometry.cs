using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CameraScript
{
    public class ParticleToGeometry : MonoBehaviour
    {
        public static Vector3 GetRandomPointOnTriangle(Vector3 A, Vector3 B, Vector3 C)
        {
            var r1     = Random.Range(0.0f, 1.0f);
            var r2     = Random.Range(0.0f, 1.0f);
            var sqrtR1 = Mathf.Sqrt(r1);
            var result = (1.0f - sqrtR1) * A + (sqrtR1 * (1.0f - r2)) * B + (sqrtR1 * r2) * C;
            return result;
        }

        #region 粒子

        public struct Particle
        {
            public Vector3 position;
        }

        public GameObject    particleTarget;
        public Material      material;
        public int           kernelIndex;
        public ComputeShader computeShader;
        public Vector3[]     particlePoint;
        public ComputeBuffer particles;

        public ComputeBuffer buffer_positions;
        public ComputeBuffer buffer_triangles;
        public ComputeBuffer buffer_triangleAreas;
        public float[]       _areaRandoms;
        public ComputeBuffer buffer_areaRandoms;
        public float[]       _r1Randoms;
        public ComputeBuffer buffer_r1Randoms;
        public float[]       _r2Randoms;
        public ComputeBuffer buffer_r2Randoms;
        public int           particleCount = 1024;
        public float         width         = 100;

        public MeshCache _meshCache = new MeshCache();

        #endregion

        private void Awake()
        {
            {
                var meshFilters = particleTarget.GetComponentsInChildren<MeshFilter>();
                _meshCache.ComputeTriangleAreas(meshFilters);
                particlePoint = new Vector3[particleCount];
                particles     = new ComputeBuffer(particleCount, Marshal.SizeOf(typeof(Particle)));
                particles.SetData(particlePoint);
                buffer_positions = new ComputeBuffer(_meshCache.positions.Length, Marshal.SizeOf(typeof(Vector3)));
                buffer_positions.SetData(_meshCache.positions);
                buffer_triangles = new ComputeBuffer(_meshCache.triangles.Length, Marshal.SizeOf(typeof(int)));
                buffer_triangles.SetData(_meshCache.triangles);
                buffer_triangleAreas = new ComputeBuffer(_meshCache.triangleAreas.Length, Marshal.SizeOf(typeof(float)));
                buffer_triangleAreas.SetData(_meshCache.triangleAreas);
                _areaRandoms       = new float[particleCount];
                buffer_areaRandoms = new ComputeBuffer(particleCount, Marshal.SizeOf(typeof(float)));
                _r1Randoms         = new float[particleCount];
                buffer_r1Randoms   = new ComputeBuffer(particleCount, Marshal.SizeOf(typeof(float)));
                _r2Randoms         = new float[particleCount];
                buffer_r2Randoms   = new ComputeBuffer(particleCount, Marshal.SizeOf(typeof(float)));
                kernelIndex        = computeShader.FindKernel("Distribution");
            }
        }

        private void Update()
        {
            for (int i = 0; i < particleCount; i++)
            {
                // var     random        = Random.Range(0.0f, _meshCache.totalArea);
                // var     triangleIndex = _meshCache.GetTriangleAreaIndex(random) * 3;
                // Vector3 p0            = _meshCache.positions[_meshCache.triangles[triangleIndex]];
                // Vector3 p1            = _meshCache.positions[_meshCache.triangles[triangleIndex + 1]];
                // Vector3 p2            = _meshCache.positions[_meshCache.triangles[triangleIndex + 2]];
                // particlePoint[i] = GetRandomPointOnTriangle(p0, p1, p2);
                _areaRandoms[i] = Random.Range(0.0f, _meshCache.totalArea);
                _r1Randoms[i]   = Random.Range(0.0f, 1.0f);
                _r2Randoms[i]   = Random.Range(0.0f, 1.0f);
            }
            
            // buffer_positions.SetData(_meshCache.positions);
            computeShader.SetBuffer(kernelIndex, "positions", buffer_positions);
            // buffer_triangles.SetData(_meshCache.triangles);
            computeShader.SetBuffer(kernelIndex, "triangles", buffer_triangles);
            // buffer_triangleAreas.SetData(_meshCache.triangleAreas);
            computeShader.SetBuffer(kernelIndex, "triangleAreas", buffer_triangleAreas);
            buffer_areaRandoms.SetData(_areaRandoms);
            computeShader.SetBuffer(kernelIndex, "areaRandoms", buffer_areaRandoms);
            buffer_r1Randoms.SetData(_r1Randoms);
            computeShader.SetBuffer(kernelIndex, "r1Randoms", buffer_r1Randoms);
            buffer_r2Randoms.SetData(_r2Randoms);
            computeShader.SetBuffer(kernelIndex, "r2Randoms", buffer_r2Randoms);
            // particles.SetData(particlePoint);
            computeShader.SetBuffer(kernelIndex, "resultPoints", particles);
            computeShader.SetInt("triangleAreasCount", _meshCache.triangleAreaCount);
            computeShader.SetFloat("totalArea", _meshCache.totalArea);
            computeShader.SetInt("totalPointCount", particleCount);
            computeShader.Dispatch(kernelIndex, particleCount / 1024 + 1, 1, 1);

            material.SetBuffer("positions", particles);
            material.SetFloat("_Width", width);
            material.SetVector("targetPosition", particleTarget.transform.position);
        }

        private void OnRenderObject()
        {
            material.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Points, particlePoint.Length);
        }

        private void OnDestroy()
        {
            if (particles != null)
            {
                particles.Release();
                particles = null;
            }
        }
    }

    public struct MeshCache
    {
        public Vector3[] positions;         //所有顶点位置
        public int[]     triangles;         //所有三角形序号
        public float[]   triangleAreas;     //所有三角形面积数组。这里为了方便通过面积找到序号，数组里面每个值都是前面所有三角形面积的和
        public int       triangleAreaCount; //三角形面积数组的长度
        public float     totalArea;         //总三角形面积

        public void ComputeTriangleAreas(MeshFilter[] meshFilters)
        {
            var prevVertexIndexCount = 0;
            var totalPositions       = new List<Vector3>();
            var totalTriangles       = new List<int>();
            var totalTriangleAreas   = new List<float>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                {
                    // 添加顶点
                    var positions = new Vector3[meshFilter.sharedMesh.vertices.Length];
                    Array.Copy(meshFilter.sharedMesh.vertices, positions, meshFilter.sharedMesh.vertices.Length);
                    totalPositions.AddRange(positions);
                }
                {
                    // 添加三角形索引
                    var triangles = new int[meshFilter.sharedMesh.triangles.Length];
                    Array.Copy(meshFilter.sharedMesh.triangles, triangles, meshFilter.sharedMesh.triangles.Length);
                    for (int i = 0; i < triangles.Length; i++)
                    {
                        triangles[i] += prevVertexIndexCount;
                    }

                    totalTriangles.AddRange(triangles);
                }

                prevVertexIndexCount += meshFilter.sharedMesh.vertices.Length;
            }

            // 计算面积
            totalArea = 0;
            for (int i = 0; i < totalTriangles.Count; i += 3)
            {
                Debug.Log(i);
                var area = GetTriangleArea(totalPositions[totalTriangles[i]], totalPositions[totalTriangles[i + 1]], totalPositions[totalTriangles[i + 2]]);
                totalArea += area;
                totalTriangleAreas.Add(totalArea);
            }

            positions         = totalPositions.ToArray();
            triangles         = totalTriangles.ToArray();
            triangleAreas     = totalTriangleAreas.ToArray();
            triangleAreaCount = triangleAreas.Length;
        }

        public int GetTriangleAreaIndex(float area)
        {
            int low  = 0;
            int high = triangleAreaCount - 1;
            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                if (triangleAreas[mid] >= area)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            if (low < triangleAreaCount)
            {
                return low;
            }
            else
            {
                return -1;
            }
        }

        public float GetTriangleArea(Vector3 A, Vector3 B, Vector3 C)
        {
            if (A == B || B == C || A == C) return 0;
            var vectorAB = B - A;
            var vectorAC = C - A;
            var dot      = Vector3.Dot(vectorAB.normalized, vectorAC.normalized);
            var angle    = Mathf.Acos(dot);
            var height   = vectorAB.magnitude * Mathf.Sin(angle);
            var width    = vectorAC.magnitude;
            return width * height * 0.5f;
        }
    }
}