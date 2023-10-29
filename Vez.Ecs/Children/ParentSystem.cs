﻿using DefaultEcs.System;
using DefaultEcs.Threading;
using DefaultEcs;

namespace Vez.Ecs.Children
{
    [With(typeof(Parent))] // add other required component types
    internal sealed class ParentSystem<T> : AEntityMultiMapSystem<T, HierarchyLevel>
    {
        public ParentSystem(World world, IParallelRunner runner)
            : base(world, runner)
        {
            HierarchyLevelSetter.Setup(world);
        }

        protected override void Update(T state, in HierarchyLevel key, in Entity entity)
        {
            entity.Get<Parent>();

            // do something
        }
    }
}