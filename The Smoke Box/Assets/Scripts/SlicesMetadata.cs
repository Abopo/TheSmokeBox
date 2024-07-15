using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;

public enum MeshSide { Positive = 0, Negative = 1 }

/// <summary>
/// An object used to manage the positive and negative side mesh data for a sliced object
/// </summary>
public class SlicesMetadata {

    private Mesh _positiveSideMesh;
    private List<Vector3> _positiveSideVertices;
    private List<int> _positiveSideTriangles;
    private List<Vector2> _positiveSideUVs;
    private List<Vector3> _positiveSideNormals;

    private Mesh _negativeSideMesh;
    private List<Vector3> _negativeSideVertices;
    private List<int> _negativeSideTriangles;
    private List<Vector2> _negativeSideUVs;
    private List<Vector3> _negativeSideNormals;

    private readonly List<Vector3> _pointsAlongPlane;
    private Plane _plane;
    private Mesh _mesh;
    bool _isSolid = true;
    bool _reverseWindTriangles = false;
    bool _useGravity = false;
    bool _shareVertices = false;
    bool _smoothVertices = false;
    private bool _createReverseTriangleWindings = false;

    public bool IsSolid { get => _isSolid; set => _isSolid = value; }
    public Mesh PositiveSideMesh { 
        get {
            if(_positiveSideMesh == null) {
                _positiveSideMesh = new Mesh();
            }

            SetMeshData(MeshSide.Positive);
            return _positiveSideMesh;
        }
    }
    public Mesh NegativeSideMesh {
        get {
            if (_negativeSideMesh == null) {
                _negativeSideMesh = new Mesh();
            }

            SetMeshData(MeshSide.Negative);
            return _negativeSideMesh;
        }
    }

    public SlicesMetadata(Plane plane, Mesh mesh, bool isSolid, bool createReverseTriangleWindings, bool shareVertices, bool smoothVertices) {
        _positiveSideTriangles = new List<int>();
        _positiveSideVertices = new List<Vector3>();
        _negativeSideTriangles = new List<int>();
        _negativeSideVertices = new List<Vector3>();
        _positiveSideUVs = new List<Vector2>();
        _negativeSideUVs = new List<Vector2>();
        _positiveSideNormals = new List<Vector3>();
        _negativeSideNormals = new List<Vector3>();
        _pointsAlongPlane = new List<Vector3>();
        _plane = plane;
        _mesh = mesh;
        _isSolid = isSolid;
        _createReverseTriangleWindings = createReverseTriangleWindings;
        _shareVertices = shareVertices;
        _smoothVertices = smoothVertices;

        ComputeNewMeshes();
    }

    /// <summary>
    /// Add the mesh data to the correct side and calculate normals
    /// </summary>
    /// <param name="side"></param>
    /// <param name="vertex1"></param>
    /// <param name="normal1"></param>
    /// <param name="uv1"></param>
    /// <param name="vertex2"></param>
    /// <param name="normal2"></param>
    /// <param name="uv2"></param>
    /// <param name="vertex3"></param>
    /// <param name="normal3"></param>
    /// <param name="uv3"></param>
    /// <param name="shareVertices"></param>
    /// <param name="addFirst"></param>
    private void AddTrianglesNormalAndUVs(MeshSide side, Vector3 vertex1, Vector3? normal1, Vector2 uv1, Vector3 vertex2, Vector3? normal2, Vector2 uv2, Vector3 vertex3, Vector3? normal3, Vector2 uv3, bool shareVertices, bool addFirst) {
        if(side == MeshSide.Positive) {
            AddTrianglesNormalsAndUVs(ref _positiveSideVertices, ref _positiveSideTriangles, ref _positiveSideNormals, ref _positiveSideUVs, vertex1, normal1, uv1, vertex2, normal2, uv2, vertex3, normal3, uv3, shareVertices, addFirst);
        } else {
            AddTrianglesNormalsAndUVs(ref _negativeSideVertices, ref _negativeSideTriangles, ref _negativeSideNormals, ref _negativeSideUVs, vertex1, normal1, uv1, vertex2, normal2, uv2, vertex3, normal3, uv3, shareVertices, addFirst);
        }
    }

    /// <summary>
    /// Adds the vertices to the mesh sets the triangles in the order that the vertices are provided.
    /// If shared vertices is false, vertices will be added to the list even if a matching vertex already exists.
    /// Does not compute normals.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    /// <param name="normals"></param>
    /// <param name="uvs"></param>
    /// <param name="vertex1"></param>
    /// <param name="normal1"></param>
    /// <param name="uv1"></param>
    /// <param name="vertex2"></param>
    /// <param name="normal2"></param>
    /// <param name="uv2"></param>
    /// <param name="vertex3"></param>
    /// <param name="normal3"></param>
    /// <param name="uv3"></param>
    /// <param name="shareVertices"></param>
    /// <param name="addFirst"></param>
    private void AddTrianglesNormalsAndUVs(ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector3> normals, ref List<Vector2> uvs, Vector3 vertex1, Vector3? normal1, Vector2 uv1, Vector3 vertex2, Vector3? normal2, Vector2 uv2, Vector3 vertex3, Vector3? normal3, Vector2 uv3, bool shareVertices, bool addFirst) {
        // tri1
        int tri1Index = vertices.IndexOf(vertex1);

        if(addFirst) {
            ShiftTriangleIndices(ref triangles);
        }

        // If the vertex already exists we just add a triangle reference to it, if not add the vert to the list and then add the tri index
        if(tri1Index > -1 && shareVertices) {
            triangles.Add(tri1Index);
        } else {
            if(normal1 == null) {
                normal1 = ComputeNormal(vertex1, vertex2, vertex3);
            }

            int? i = null;
            if(addFirst) {
                i = 0;
            }

            AddVertNormalUV(ref vertices, ref normals, ref uvs, ref triangles, vertex1, (Vector3)normal1, uv1, i);
        }

        // tri2
        int tri2Index = vertices.IndexOf(vertex2);

        if (tri2Index > -1 && shareVertices) {
            triangles.Add(tri2Index);
        } else {
            if (normal2 == null) {
                normal2 = ComputeNormal(vertex2, vertex3, vertex1);
            }

            int? i = null;
            if (addFirst) {
                i = 1;
            }

            AddVertNormalUV(ref vertices, ref normals, ref uvs, ref triangles, vertex2, (Vector3)normal2, uv2, i);
        }

        // tri3
        int tri3Index = vertices.IndexOf(vertex3);

        if (tri3Index > -1 && shareVertices) {
            triangles.Add(tri3Index);
        } else {
            if (normal3 == null) {
                normal3 = ComputeNormal(vertex3, vertex1, vertex2);
            }

            int? i = null;
            if (addFirst) {
                i = 2;
            }

            AddVertNormalUV(ref vertices, ref normals, ref uvs, ref triangles, vertex3, (Vector3)normal3, uv3, i);
        }
    }

    private void AddVertNormalUV(ref List<Vector3> vertices, ref List<Vector3> normals, ref List<Vector2> uvs, ref List<int> triangles, Vector3 vertex, Vector3 normal, Vector2 uv, int? index) {
        if(index != null) {
            int i = (int)index;
            vertices.Insert(i, vertex);
            normals.Insert(i, normal);
            uvs.Insert(i, uv);
            triangles.Insert(i, i);
        } else {
            vertices.Add(vertex);
            normals.Add(normal);
            uvs.Add(uv);
            triangles.Add(vertices.IndexOf(vertex));
        }
    }

    private void ShiftTriangleIndices(ref List<int> triangles) {
        for(int j = 0; j < triangles.Count; j += 3) {
            triangles[j] += 3;
            triangles[j + 1] += 3;
            triangles[j + 2] += 3;
        }
    }

    private void AddReverseTriangleWinding() {
        int positiveVertsStartIndex = _positiveSideVertices.Count;
        // Duplicate the original vertices
        _positiveSideVertices.AddRange(_positiveSideVertices);
        _positiveSideUVs.AddRange(_positiveSideUVs);
        _positiveSideNormals.AddRange(FlipNormals(_positiveSideNormals));

        int numPositiveTriangles = _positiveSideTriangles.Count;

        // Add reverse windings
        for(int i = 0; i < numPositiveTriangles; i += 3) {
            _positiveSideTriangles.Add(positiveVertsStartIndex + _positiveSideTriangles[i]);
            _positiveSideTriangles.Add(positiveVertsStartIndex + _positiveSideTriangles[i + 2]);
            _positiveSideTriangles.Add(positiveVertsStartIndex + _positiveSideTriangles[i + 1]);
        }

        int negativeVertsStartIndex = _negativeSideVertices.Count;
        // Duplicate the original vertices
        _negativeSideVertices.AddRange(_negativeSideVertices);
        _negativeSideUVs.AddRange(_negativeSideUVs);
        _negativeSideNormals.AddRange(FlipNormals(_negativeSideNormals));

        int numnegativeTriangles = _negativeSideTriangles.Count;

        // Add reverse windings
        for (int i = 0; i < numnegativeTriangles; i += 3) {
            _negativeSideTriangles.Add(negativeVertsStartIndex + _negativeSideTriangles[i]);
            _negativeSideTriangles.Add(negativeVertsStartIndex + _negativeSideTriangles[i + 2]);
            _negativeSideTriangles.Add(negativeVertsStartIndex + _negativeSideTriangles[i + 1]);
        }
    }

    /// <summary>
    /// Join the points along the plane to the halfway point
    /// </summary>
    private void JoinPointsAlongPlane() {
        Vector3 halfway = GetHalfwayPoint(out float distance);

        for (int i = 0; i < _pointsAlongPlane.Count; i += 2) {
            Vector3 firstVertex;
            Vector3 secondVertex;

            firstVertex = _pointsAlongPlane[i];
            secondVertex = _pointsAlongPlane[i + 1];

            Vector3 normal3 = ComputeNormal(halfway, secondVertex, firstVertex);
            normal3.Normalize();

            var direction = Vector3.Dot(normal3, _plane.normal);

            if(direction > 0) {
                AddTrianglesNormalAndUVs(MeshSide.Positive, halfway, -normal3, Vector2.zero, firstVertex, -normal3, Vector2.zero, secondVertex, -normal3, Vector2.zero, false, true);
                AddTrianglesNormalAndUVs(MeshSide.Negative, halfway, normal3, Vector2.zero, secondVertex, normal3, Vector2.zero, firstVertex, normal3, Vector2.zero, false, true);
            } else {
                AddTrianglesNormalAndUVs(MeshSide.Positive, halfway, normal3, Vector2.zero, firstVertex, normal3, Vector2.zero, secondVertex, normal3, Vector2.zero, false, true);
                AddTrianglesNormalAndUVs(MeshSide.Negative, halfway, -normal3, Vector2.zero, secondVertex, -normal3, Vector2.zero, firstVertex, -normal3, Vector2.zero, false, true);
            }
        }
    }

    /// <summary>
    /// For all the points added along the plane cut, get the halfway between the first and furthest point.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetHalfwayPoint(out float distance) {
        if(_pointsAlongPlane.Count > 0) {
            Vector3 firstPoint = _pointsAlongPlane[0];
            Vector3 furthestPoint = Vector3.zero;
            distance = 0f;

            float currentDistance = 0f;
            foreach (Vector3 point in _pointsAlongPlane) {
                currentDistance = Vector3.Distance(firstPoint, point);

                if(currentDistance > distance) {
                    distance = currentDistance;
                    furthestPoint = point;
                }
            }

            return Vector3.Lerp(firstPoint, furthestPoint, 0.5f);
        } else {
            distance = 0;
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Setup the mesh object for the specified side
    /// </summary>
    /// <param name="side"></param>
    private void SetMeshData(MeshSide side) {
        if(side == MeshSide.Positive) {
            _positiveSideMesh.vertices = _positiveSideVertices.ToArray();
            _positiveSideMesh.triangles = _positiveSideTriangles.ToArray();
            _positiveSideMesh.normals = _positiveSideNormals.ToArray();
            _positiveSideMesh.uv = _positiveSideUVs.ToArray();
        } else {
            _negativeSideMesh.vertices = _negativeSideVertices.ToArray();
            _negativeSideMesh.triangles = _negativeSideTriangles.ToArray();
            _negativeSideMesh.normals = _negativeSideNormals.ToArray();
            _negativeSideMesh.uv = _negativeSideUVs.ToArray();
        }
    }

    /// <summary>
    /// Compute the positive and negative meshes based on the plane and mesh
    /// </summary>
    private void ComputeNewMeshes() {
        int[] meshTriangles = _mesh.triangles;
        Vector3[] meshVerts = _mesh.vertices;
        Vector3[] meshNormals = _mesh.normals;
        Vector2[] meshUVs = _mesh.uv;

        for(int i = 0; i < meshTriangles.Length; i += 3) {
            // We need the verts in order so that we know which way to wind our new mesh trianlges.
            Vector3 vert1 = meshVerts[meshTriangles[i]];
            int vert1Index = Array.IndexOf(meshVerts, vert1);
            Vector2 uv1 = meshUVs[vert1Index];
            Vector3 normal1 = meshNormals[vert1Index];
            bool vert1Side = _plane.GetSide(vert1);

            Vector3 vert2 = meshVerts[meshTriangles[i + 1]];
            int vert2Index = Array.IndexOf(meshVerts, vert2);
            Vector2 uv2 = meshUVs[vert2Index];
            Vector3 normal2 = meshNormals[vert2Index];
            bool vert2Side = _plane.GetSide(vert2);

            Vector3 vert3 = meshVerts[meshTriangles[i + 2]];
            int vert3Index = Array.IndexOf(meshVerts, vert3);
            Vector3 uv3 = meshUVs[vert3Index];
            Vector3 normal3 = meshNormals[vert3Index];
            bool vert3Side = _plane.GetSide(vert3);

            // All verts are on the same side
            if(vert1Side == vert2Side && vert2Side == vert3Side) {
                // Add the relevant triangle
                MeshSide side = (vert1Side) ? MeshSide.Positive : MeshSide.Negative;
                AddTrianglesNormalAndUVs(side, vert1, normal1, uv1, vert2, normal2, uv2, vert3, normal3, uv3, true, false);
            } else {
                // We need the two points where the plane intersects the triangle
                Vector3 intersection1;
                Vector3 intersection2;

                Vector2 intersection1UV;
                Vector2 intersection2UV;

                MeshSide side1 = (vert1Side) ? MeshSide.Positive : MeshSide.Negative;
                MeshSide side2 = (vert1Side) ? MeshSide.Negative : MeshSide.Positive;

                if(vert1Side == vert2Side) {
                    // Cast a ray from v2 to v3 and from v3 to v1 to get the intersections
                    intersection1 = GetRayPlaneIntersectionPointAndUV(vert2, uv2, vert3, uv3, out intersection1UV);
                    intersection2 = GetRayPlaneIntersectionPointAndUV(vert3, uv3, vert1, uv1, out intersection2UV);

                    // Add the positive or negative triangles
                    AddTrianglesNormalAndUVs(side1, vert1, null, uv1, vert2, null, uv2, intersection1, null, intersection1UV, _shareVertices, false);
                    AddTrianglesNormalAndUVs(side1, vert1, null, uv1, intersection1, null, intersection1UV, intersection2, null, intersection2UV, _shareVertices, false);

                    AddTrianglesNormalAndUVs(side2, intersection1, null, intersection1UV, vert3, null, uv3, intersection2, null, intersection2UV, _shareVertices, false);
                
                // vert1 and 3 are on the same side
                } else if(vert1Side == vert3Side) {
                    // Cast a ray from v1 to v2 and from v2 to v3 to get the intersections
                    intersection1 = GetRayPlaneIntersectionPointAndUV(vert1, uv1, vert2, uv2, out intersection1UV);
                    intersection2 = GetRayPlaneIntersectionPointAndUV(vert2, uv2, vert3, uv3, out intersection2UV);

                    // Add the positive or negative triangles
                    AddTrianglesNormalAndUVs(side1, vert1, null, uv1, intersection1, null, intersection1UV, vert3, null, uv3, _shareVertices, false);
                    AddTrianglesNormalAndUVs(side1, intersection1, null, intersection1UV, intersection2, null, intersection2UV, vert3, null, uv3, _shareVertices, false);

                    AddTrianglesNormalAndUVs(side2, intersection1, null, intersection1UV, vert2, null, uv2, intersection2, null, intersection2UV, _shareVertices, false);
                
                // vert1 is alone
                } else {
                    // Cast a ray from v1 to v2 and from v1 to v3 to get the intersections
                    intersection1 = GetRayPlaneIntersectionPointAndUV(vert1, uv1, vert2, uv2, out intersection1UV);
                    intersection2 = GetRayPlaneIntersectionPointAndUV(vert1, uv1, vert3, uv3, out intersection2UV);

                    // Add the positive or negative triangles
                    AddTrianglesNormalAndUVs(side1, vert1, null, uv1, intersection1, null, intersection1UV, intersection2, null, intersection2UV, _shareVertices, false);
                    
                    AddTrianglesNormalAndUVs(side1, intersection1, null, intersection1UV, vert2, null, uv2, vert3, null, uv3, _shareVertices, false);
                    AddTrianglesNormalAndUVs(side2, intersection1, null, intersection1UV, vert3, null, uv3, intersection2, null, intersection2UV, _shareVertices, false);
                }

                // Add the newly created points on the plane
                _pointsAlongPlane.Add(intersection1);
                _pointsAlongPlane.Add(intersection2);
            }

            // if the object is solid, join the new points along the plane otherwise do the reverse winding
            if(_isSolid) {
                JoinPointsAlongPlane();
            } else if(_createReverseTriangleWindings) {
                AddReverseTriangleWinding();
            }

            if(_smoothVertices) {
                SmoothVertices();
            }
        }
    }

    /// <summary>
    /// Casts a ray from vertex1 to vertex2 and gets the point of intersection with the plane, calculates the new uv as well
    /// </summary>
    /// <param name="vertex1"></param>
    /// <param name="vertex1UV"></param>
    /// <param name="vertex2"></param>
    /// <param name="vertex2UV"></param>
    /// <param name="uv"></param>
    /// <returns></returns>
    private Vector3 GetRayPlaneIntersectionPointAndUV(Vector3 vertex1, Vector2 vertex1UV, Vector3 vertex2, Vector2 vertex2UV, out Vector2 uv) {
        float distance = GetDistanceRelativeToPlane(vertex1, vertex2, out Vector3 pointOfIntersection);
        uv = InterpolateUVs(vertex1UV, vertex2UV, distance);
        return pointOfIntersection;
    }

    /// <summary>
    /// Computes the distance based on the plane
    /// </summary>
    /// <param name="vertex1"></param>
    /// <param name="vertex2"></param>
    /// <param name="pointOfIntersection"></param>
    /// <returns></returns>
    private float GetDistanceRelativeToPlane(Vector3 vertex1, Vector3 vertex2, out Vector3 pointOfIntersection) {
        Ray ray = new Ray(vertex1, (vertex2 - vertex1));
        _plane.Raycast(ray, out float distance);
        pointOfIntersection = ray.GetPoint(distance);
        return distance;
    }

    /// <summary>
    /// Get a uv between the two provided uvs by the distance.
    /// </summary>
    /// <param name="uv1"></param>
    /// <param name="uv2"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private Vector2 InterpolateUVs(Vector2 uv1, Vector2 uv2, float distance) {
        Vector2 uv = Vector2.Lerp(uv1, uv2, distance);
        return uv;
    }

    /// <summary>
    /// Gets the point perpendicular to the face defined by the provided vertices.
    /// </summary>
    /// <param name="vertex1"></param>
    /// <param name="vertex2"></param>
    /// <param name="vertex3"></param>
    /// <returns></returns>
    private Vector3 ComputeNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3) {
        Vector3 side1 = vertex2 - vertex1;
        Vector3 side2 = vertex3 - vertex1;

        Vector3 normal = Vector3.Cross(side1, side2);

        return normal;
    }

    /// <summary>
    /// Reverse the normals in a given list
    /// </summary>
    /// <param name="currentNormals"></param>
    /// <returns></returns>
    private List<Vector3> FlipNormals(List<Vector3> currentNormals) {
        List<Vector3> flippedNormals = new List<Vector3>();

        foreach(Vector3 normal in currentNormals) {
            flippedNormals.Add(-normal);
        }

        return flippedNormals;
    }

    private void SmoothVertices() {
        DoSmoothing(ref _positiveSideVertices, ref _positiveSideNormals, ref _positiveSideTriangles);
        DoSmoothing(ref _negativeSideVertices, ref _negativeSideNormals, ref _negativeSideTriangles);
    }

    private void DoSmoothing(ref List<Vector3> vertices, ref List<Vector3> normals, ref List<int> triangles) {
        normals.ForEach(x =>
        {
            x = Vector3.zero;
        });

        for (int i = 0; i < triangles.Count;  i += 3) {
            int vertIndex1 = triangles[i];
            int vertIndex2 = triangles[i + 1];
            int vertIndex3 = triangles[i + 2];

            Vector3 triangleNormal = ComputeNormal(vertices[vertIndex1], vertices[vertIndex2], vertices[vertIndex3]);

            normals[vertIndex1] += triangleNormal;
            normals[vertIndex2] += triangleNormal;
            normals[vertIndex3] += triangleNormal;
        }

        normals.ForEach(x =>
        {
            x.Normalize();
        });
    }
}
