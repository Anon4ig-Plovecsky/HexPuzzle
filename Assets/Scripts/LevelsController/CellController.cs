using UnityEngine;
using System;

namespace LevelsController
{
    public class CellController : MonoBehaviour
    {
        private const string CorrectlyString = "[Correctly]";
        private string _nameDefault, _nameOtherCollider;
        private ParticleSystem _effectGameObject;
        private Renderer _rendererGameObject;
        private Vector3 _size;
        private int _index;
        private void Start()
        {
            _nameDefault = name;
            _index = Convert.ToInt32(name.Split('(', ')')[1]);
            _size = GetComponent<BoxCollider>().size;
            _rendererGameObject = transform.GetChild(1).GetComponent<Renderer>();
            _effectGameObject = transform.GetChild(0).GetComponent<ParticleSystem>();
        }
        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.name.Contains("PartOfPainting")) return;
            var rotation = otherCollider.transform.rotation.normalized.eulerAngles;
            if ((!(Math.Abs(rotation.x) % 90 <= 15) &&
                 !(Math.Abs(rotation.x) % 90 >= 75)) ||
                (!(Math.Abs(rotation.y) % 90 <= 15) &&
                 !(Math.Abs(rotation.y) % 90 >= 75)) ||
                (!(Math.Abs(rotation.z) % 90 <= 15) &&
                 !(Math.Abs(rotation.z) % 90 >= 75))) return;
            if (!(otherCollider.transform.position.x < transform.position.x + _size.x / 2f)
                || !(otherCollider.transform.position.x > transform.position.x - _size.x / 2f)
                || !(otherCollider.transform.position.z < transform.position.z + _size.z / 2f)
                || !(otherCollider.transform.position.z > transform.position.z - _size.z / 2f)) return;
            _nameOtherCollider = otherCollider.name;
            _rendererGameObject.enabled = false;
            _effectGameObject.Stop();
            if (_index != Convert.ToInt32(otherCollider.name.Split('(', ')')[1])) return;
            if(CheckCorrectly(rotation))
                name = $"{_nameDefault} {CorrectlyString}";
        }
        private void OnTriggerExit(Collider otherCollider)
        {
            if (_rendererGameObject.enabled || _nameOtherCollider != otherCollider.name) return;
            _nameOtherCollider = "";
            name = _nameDefault;
            _rendererGameObject.enabled = true;
            _effectGameObject.Play();
        }
        private static bool CheckCorrectly(Vector3 rotation)
        {
            return (Math.Abs(rotation.x) % 360 < 5 || Math.Abs(rotation.x) % 360 > 355) &&
                   (Math.Abs(rotation.z) % 360 < 5 || Math.Abs(rotation.z) % 360 > 355) &&
                   (Math.Abs(rotation.y) % 360 < 15 || Math.Abs(rotation.y) % 360 > 345);
        }
    }
}