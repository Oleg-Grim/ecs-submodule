﻿#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntityVersions {

        [ME.ECS.Serializer.SerializeField]
        private NativeBufferArray<ushort> values;
        private static ushort defaultValue;

        public override int GetHashCode() {

            var hash = 0;
            for (int i = 0; i < this.values.Length; ++i) {
                hash ^= (int)(this.values.arr[i] + 100000u);
            }

            return hash;

        }

        public void Recycle() {

            PoolArrayNative<ushort>.Recycle(ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(int capacity) {

            ArrayUtils.Resize(capacity, ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity) {

            var id = entity.id;
            ArrayUtils.Resize(id, ref this.values, true);

        }

        public void CopyFrom(EntityVersions other) {

            ArrayUtils.Copy(in other.values, ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(int entityId) {

            return ref this.values.arr.GetRef(entityId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(in Entity entity) {

            var id = entity.id;
            if (id >= this.values.Length) return ref EntityVersions.defaultValue;
            return ref this.values.arr.GetRef(id);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(in Entity entity) {

            unchecked {
                ++this.values.arr.GetRef(entity.id);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(int entityId) {

            unchecked {
                ++this.values.arr.GetRef(entityId);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(in Entity entity) {

            this.values.arr.GetRef(entity.id) = 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(int entityId) {

            this.Validate(entityId);
            this.values.arr.GetRef(entityId) = 0;

        }

    }

}