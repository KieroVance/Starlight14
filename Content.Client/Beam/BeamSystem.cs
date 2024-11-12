﻿using Content.Client.Beam.Components;
using Content.Shared.Beam;
using Content.Shared.Beam.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed.TypeParsers;

namespace Content.Client.Beam;

public sealed class BeamSystem : SharedBeamSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<BeamVisualizerEvent>(BeamVisualizerMessage);
    }

    //TODO: Sometime in the future this needs to be replaced with tiled sprites
    private void BeamVisualizerMessage(BeamVisualizerEvent args)
    {
        var beam = GetEntity(args.Beam);

        if (TryComp<SpriteComponent>(beam, out var sprites))
        {
            sprites.Rotation = args.UserAngle;

            if (args.BodyState != null && Prototype(beam)?.ID == "Lightning")
            {
                sprites.LayerSetState(0, args.BodyState);
                sprites.LayerSetShader(0, args.Shader);
            }
        }
    }
}
