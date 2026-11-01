using Assets.Project.Scripts.Status;
using UnityEngine;

public static class StatusEffectRequestFactory
{
    public static StatusEffectRequest[] FromPayloads
        (
            StatusEffectPayload[] payloads,
            GameObject source
        )
    {
        if (payloads == null || payloads.Length == 0) return null;

        var requests = new StatusEffectRequest[payloads.Length];
        for (int i = 0; i < payloads.Length; i++)
        {
            var payload = payloads[i];
            requests[i] = new StatusEffectRequest
                (
                    payload.Type,
                    payload.Duration,
                    payload.TickInterval,
                    payload.Power,
                    source,
                    payload.Stacks
                );
        }

        return requests;
    }
}
