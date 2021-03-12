﻿#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using ME.ECS.Collections;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataBuffer<T> where T : struct, IStructComponent {

        private Unity.Collections.NativeArray<T> arr;
        private Unity.Collections.NativeArray<byte> ops;
        private readonly int minIdx;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx, int length, Unity.Collections.Allocator allocator) {

            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            this.minIdx = minIdx;
            this.arr = new Unity.Collections.NativeArray<T>(reg.components.data.arr, allocator);
            this.ops = new Unity.Collections.NativeArray<byte>(reg.components.data.arr.Length, allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) : this(world, arr, minIdx, maxIdx, maxIdx - minIdx, Unity.Collections.Allocator.Persistent) {}

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int Push(World world, ME.ECS.Collections.BufferArray<Entity> arr, int max, Unity.Collections.NativeArray<bool> inFilter) {

            var changedCount = 0;
            var isTag = WorldUtilities.IsComponentAsTag<T>();
            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            for (int i = this.minIdx; i <= max; ++i) {

                if (inFilter[i] == false) continue;
                if (this.ops[i] == 0) continue;

                var entity = arr.arr[i];
                if ((this.ops[i] & 0x4) != 0) {

                    if (isTag == false) reg.components[entity.id] = default;
                    ref var state = ref reg.componentsStates.arr[entity.id];
                    if (state > 0) {

                        state = 0;
                        if (world.currentState.filters.HasInAnyFilter<T>() == true) {

                            world.currentState.storage.archetypes.Remove<T>(in entity);

                        }

                        System.Threading.Interlocked.Decrement(ref world.currentState.structComponents.count);
                        
                    }
                    
                    world.currentState.storage.versions.Increment(in entity);
                    ++changedCount;
                    
                } else if ((this.ops[i] & 0x2) != 0) {

                    if (isTag == false) reg.components[entity.id] = this.arr[entity.id];
                    ref var state = ref reg.componentsStates.arr[entity.id];
                    if (state == 0) {

                        state = 1;
                        if (world.currentState.filters.HasInAnyFilter<T>() == true) {

                            world.currentState.storage.archetypes.Set<T>(in entity);

                        }

                        System.Threading.Interlocked.Increment(ref world.currentState.structComponents.count);

                    }

                    world.currentState.storage.versions.Increment(in entity);
                    ++changedCount;

                }
                
            }
            
            this.Dispose();

            return changedCount;

        }

        public void Dispose() {
            
            this.arr.Dispose();
            this.ops.Dispose();
            
        }

        public void Remove(int entityId) {

            this.ops[entityId] |= 0x4;
            
        }

        public void Set(int entityId, in T data) {

            this.ops[entityId] |= 0x2;
            this.arr[entityId] = data;
            
        }

        public ref T Get(int entityId) {

            this.ops[entityId] |= 0x2;
            return ref this.arr.GetRef(entityId);

        }

        public ref readonly T Read(int entityId) {

            this.ops[entityId] |= 0x1;
            return ref this.arr.GetRef(entityId);

        }

    }

}