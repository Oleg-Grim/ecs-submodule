﻿#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define EDITOR_ARRAY

namespace ME.ECS.Collections {

    #if BUFFER_SLICED_DISABLED
    [System.Serializable]
    public struct NativeBufferArraySliced<T> : IBufferArraySliced where T : struct {

        public NativeBufferArray<T> data;

        public bool isCreated => this.data.isCreated;
        
        public int Length {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => this.data.Length;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public NativeBufferArraySliced<T> Merge() => this;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public readonly unsafe void* GetUnsafePtr() {
            return this.data.GetUnsafePtr();
        }
        
        public ref T this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => ref this.data[index];
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public NativeBufferArraySliced<T> Dispose() {

            this.data.Dispose();
            this.data = default;
            return default;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public NativeBufferArraySliced<T> Resize(int index, bool resizeWithOffset, out bool result) {

            result = NativeArrayUtils.Resize(index, ref this.data, resizeWithOffset);
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(NativeBufferArraySliced<T> other) {
            
            NativeArrayUtils.Copy(other.data, ref this.data);
            
        }

    }
    #else
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public struct NativeBufferArraySliced<T> : IBufferArraySliced where T : struct {

        private const int BUCKET_SIZE = 4;

        public NativeBufferArray<T> data;
        public BufferArray<NativeBufferArray<T>> tails;
        public int tailsLength;
        public bool isCreated;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public readonly unsafe void* GetUnsafePtr() {
            return this.data.GetUnsafePtr();
        }

        public int Length {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get => this.data.Length + this.tailsLength;
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced(NativeBufferArray<T> arr) {

            this.isCreated = true;
            this.data = arr;
            this.tails = default;
            this.tailsLength = 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced(int length) {

            this.isCreated = true;
            this.data = new NativeBufferArray<T>(length);
            this.tails = default;
            this.tailsLength = 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private NativeBufferArraySliced(NativeBufferArray<T> arr, BufferArray<NativeBufferArray<T>> tails) {

            this.isCreated = true;
            this.data = arr;
            this.tails = tails;
            this.tailsLength = 0;
            for (int i = 0, length = tails.Length; i < length; ++i) {

                var tail = tails.arr[i];
                this.tailsLength += tail.Length;

            }

        }

        public ref T this[int index] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                var data = this.data;
                if (index >= data.Length) {

                    // Look into tails
                    var tails = this.tails;
                    var arr = tails.arr;
                    index -= data.Length;
                    for (int i = 0, length = tails.Length; i < length; ++i) {

                        ref var tail = ref arr[i];
                        var len = tail.Length;
                        if (index < len) return ref tail.arr.GetRef(index);
                        index -= len;

                    }

                }

                return ref data[index];
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced<T> CopyFrom<TCopy>(in NativeBufferArraySliced<T> other, in TCopy copy) where TCopy : IArrayElementCopy<T> {

            ref var data = ref this.data;
            this.isCreated = other.isCreated;
            //var tails = this.tails;
            NativeArrayUtils.Copy(other.data, ref data, copy);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy<TCopy>() { elementCopy = copy });
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced<T> CopyFrom(in NativeBufferArraySliced<T> other) {

            ref var data = ref this.data;
            this.isCreated = other.isCreated;
            //var tails = this.tails;
            NativeArrayUtils.Copy(in other.data, ref data);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy());
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced<T> Resize(int index, bool resizeWithOffset, out bool result) {

            if (index >= this.Length) {

                // Do we need any tail?
                ref var tails = ref this.tails;
                // Look into tails
                var ptr = this.data.Length;
                for (int i = 0, length = this.tails.Length; i < length; ++i) {

                    // For each tail determine do we need to resize any tail to store index?
                    var tail = tails.arr[i];
                    ptr += tail.Length;
                    if (index >= ptr) continue;

                    // We have found tail without resize needed
                    // Tail was found - we do not need to resize
                    result = false;
                    return this;

                }

                // Need to add new tail and resize tails container
                var idx = tails.Length;
                var size = this.Length;
                ArrayUtils.Resize(idx, ref tails, resizeWithOffset);
                var bucketSize = index + NativeBufferArraySliced<T>.BUCKET_SIZE - size;
                tails.arr[idx] = new NativeBufferArray<T>(bucketSize);//PoolArray<T>.Spawn(bucketSize, realSize: false);
                this.tailsLength += bucketSize;
                result = true;
                this.isCreated = true;
                return this;

            }

            // We dont need to resize any
            result = false;
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced<T> Merge() {

            if (this.tails.isCreated == false || this.tails.Length == 0) {

                return this;

            }

            //var arr = PoolArrayNative<T>.Spawn(this.Length);
            //if (this.data.isCreated == true) NativeArrayUtils.Copy(this.data, 0, ref arr, 0, this.data.Length);
            var ptr = this.data.Length;
            NativeArrayUtils.Resize(this.Length - 1, ref this.data);
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails.arr[i];
                if (tail.isCreated == false) continue;

                NativeArrayUtils.Copy(tail, 0, ref this.data, ptr, tail.Length);
                ptr += tail.Length;
                tail.Dispose();

            }

            this.tails = default;
            this.tailsLength = 0;
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArraySliced<T> Dispose() {

            this.data.Dispose();
            this.data = default;
            
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                var tail = this.tails.arr[i];
                this.tails.arr[i] = tail.Dispose();

            }

            this.tails.Dispose();
            this.tails = default;

            this.isCreated = false;
            
            return default;

        }

    }
    #endif

}