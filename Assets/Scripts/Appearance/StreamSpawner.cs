using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tools;

public class StreamSpawner : MonoBehaviour
{
    public GameObject streamerPrefab;
    public int TotalSlots;
    public int SlotsPerContact;
    HashSet<Collider> _contacts = new HashSet<Collider>();
    HashSet<Collider> contacts = new HashSet<Collider>();
    List<Ref<(GameObject streamer, float time, Collider lastTarget, Vector3 nullTarget, bool Taken)>> slots = new List<Ref<(GameObject, float, Collider, Vector3, bool)>>();
    void FixedUpdate()
    {
        contacts = new HashSet<Collider>(_contacts);
        _contacts.Clear();
    }
    void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
            _contacts.Add(other);
    }
    void Update()
    {
        transform.eulerAngles = default;
        var allocations = new List<(Collider collider, int slots)>();
        var unusedSlots = TotalSlots - SlotsPerContact * contacts.Count;
        var extraSlots = -unusedSlots;
        var leftToCount = contacts.Count;
        foreach (var contact in contacts)
        {
            var reduce = 0;
            if (extraSlots > 0)
                reduce = extraSlots / leftToCount;
            leftToCount--;
            extraSlots -= reduce;
            allocations.Add((contact, SlotsPerContact - reduce));
        }
        for (int i = 0; i < unusedSlots; i++)
            allocations.Add((null, 1));
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var v = slots[i].value;
            v.time += Time.deltaTime;
            if (v.time < 2)
                slots[i].value = v;
            else
            {
                Destroy(slots[i].value.streamer);
                slots.RemoveAt(i);
            }
        }
        foreach (var slot in slots)
        {
            var i = slot.value.lastTarget ? allocations.FindIndex(x => x.collider == slot.value.lastTarget) : -1;
            if (i == -1 || allocations[i].slots == 0)
            {
                slot.value.lastTarget = null;
                slot.value.Taken = false;
            } else
            {
                var v = allocations[i];
                v.slots--;
                allocations[i] = v;
                slot.value.Taken = true;
            }
        }
        for (int i = 0; i < allocations.Count; i++)
        {
            if (allocations[i].slots == 0)
                continue;
            while (allocations[i].slots > 0)
            {
                var v = allocations[i];
                v.slots--;
                allocations[i] = v;
                var index = slots.FindIndex(x => !x.value.Taken);
                if (index == -1)
                {
                    var obj = Instantiate(streamerPrefab, transform);
                    obj.transform.localRotation = Quaternion.Euler(GetRandomEular());
                    slots.Add((obj, Random.Range(0,0.2f), allocations[i].collider, GetRandomEular(), true));
                }
                else
                {
                    slots[index].value.Taken = true;
                    slots[index].value.lastTarget = allocations[i].collider;
                }
            }
        }
        var p = transform.position;
        foreach (var slot in slots)
            if (slot.value.lastTarget)
            {
                var point = slot.value.lastTarget.ClosestPoint(p);
                if (point == Vector3.zero)
                    Debug.LogWarning(slot.value.lastTarget);
                else
                    slot.value.streamer.transform.localRotation = Quaternion.RotateTowards(slot.value.streamer.transform.localRotation, Quaternion.LookRotation(transform.InverseTransformPoint(point)), 720 * Time.deltaTime);

            }
            else
            {
                slot.value.streamer.transform.localRotation = Quaternion.RotateTowards(slot.value.streamer.transform.localRotation, Quaternion.Euler(slot.value.nullTarget), 180 * Time.deltaTime);
                if (Quaternion.Angle(slot.value.streamer.transform.localRotation, Quaternion.Euler(slot.value.nullTarget)) < 1)
                    slot.value.time = 2;
            }
    }

}
