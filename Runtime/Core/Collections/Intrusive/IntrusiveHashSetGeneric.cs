﻿#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    public interface IIntrusiveHashSetGeneric<T> where T : struct, System.IEquatable<T> {

        int Count { get; }

        void Add(in T entityData);
        bool Remove(in T entityData);
        int RemoveAll(in T entityData);
        void Clear();
        bool Contains(in T entityData);

        BufferArray<T> ToArray();
        IntrusiveHashSetGeneric<T>.Enumerator GetEnumerator();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct IntrusiveHashSetGeneric<T> : IIntrusiveHashSetGeneric<T> where T : struct, System.IEquatable<T> {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private IntrusiveHashSetGeneric<T> hashSet;
            private int bucketIndex;
            private IntrusiveListGeneric<T>.Enumerator listEnumerator;

            T System.Collections.Generic.IEnumerator<T>.Current => this.listEnumerator.Current;
            public ref T Current => ref this.listEnumerator.Current;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public Enumerator(IntrusiveHashSetGeneric<T> hashSet) {

                this.hashSet = hashSet;
                this.bucketIndex = 0;
                this.listEnumerator = default;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                while (this.bucketIndex <= this.hashSet.buckets.Length) {

                    if (this.listEnumerator.MoveNext() == true) {

                        return true;

                    }

                    var bucket = this.hashSet.buckets[this.bucketIndex];
                    if (bucket.IsAlive() == true) {

                        var node = bucket.Read<IntrusiveHashSetBucketGeneric<T>>();
                        this.listEnumerator = node.list.GetEnumerator();

                    }

                    ++this.bucketIndex;

                }

                return false;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() {

                this.bucketIndex = 0;
                this.listEnumerator = default;

            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            public void Dispose() { }

        }

        [ME.ECS.Serializer.SerializeField]
        private Entity data;
        
        private StackArray10<Entity> buckets {
            readonly get {
                if (this.data == Entity.Null) return new StackArray10<Entity>(10);
                return this.data.Read<IntrusiveHashSetData>().buckets;
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveHashSetData>().buckets = value;
            }
        }
        
        private int count {
            readonly get {
                if (this.data == Entity.Null) return 0;
                return this.data.Read<IntrusiveHashSetData>().count;   
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveHashSetData>().count = value;
            }
        }

        public readonly int Count => this.count;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateData() {

            IntrusiveHashSetGeneric<T>.InitializeComponents();
            
            if (this.data == Entity.Null) {
                this.data = new Entity(EntityFlag.None);
                this.data.ValidateDataBlittable<IntrusiveHashSetData>();
                this.data.Set(new IntrusiveHashSetData());
            }
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly BufferArray<T> ToArray() {

            var arr = PoolArray<T>.Spawn(this.count);
            var i = 0;
            foreach (var entity in this) {

                arr.arr[i++] = entity;

            }

            return arr;

        }

        /// <summary>
        /// Find an element.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly bool Contains(in T entityData) {

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return false;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucketGeneric<T>>();
            return bucketList.list.Contains(entityData);

        }

        /// <summary>
        /// Clear the list.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear() {

            for (int i = 0; i < this.buckets.Length; ++i) {

                var bucket = this.buckets[i];
                if (bucket.IsAlive() == true) {

                    ref var data = ref bucket.Get<IntrusiveHashSetBucketGeneric<T>>();
                    data.list.Clear();

                }

            }

            this.count = 0;

        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose() {

            if (Worlds.isInDeInitialization == true) return;
            
            this.Clear();
            var buckets = this.buckets;
            for (int i = 0; i < buckets.Length; ++i) {
                
                if (buckets[i].IsAlive() == true) buckets[i].Destroy();
                buckets[i] = default;

            }

            this.buckets = buckets;

        }

        /// <summary>
        /// Remove data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Remove(in T entityData) {

            IntrusiveHashSetGeneric<T>.Initialize(ref this);

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return false;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucketGeneric<T>>();
            if (bucketList.list.Remove(entityData) == true) {

                --this.count;
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove all nodes data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveAll(in T entityData) {

            IntrusiveHashSetGeneric<T>.Initialize(ref this);

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return 0;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucketGeneric<T>>();
            var count = bucketList.list.RemoveAll(in entityData);
            this.count -= count;
            return count;

        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in T entityData) {

            IntrusiveHashSetGeneric<T>.InitializeComponents();
            IntrusiveHashSetGeneric<T>.Initialize(ref this);

            var buckets = this.buckets;
            var bucket = (entityData.GetHashCode() & 0x7fffffff) % buckets.Length;
            var bucketEntity = buckets[bucket];
            if (bucketEntity.IsAlive() == false) {
                bucketEntity = buckets[bucket] = new Entity(EntityFlag.None);
                bucketEntity.ValidateData<IntrusiveHashSetBucketGeneric<T>>();
            }

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucketGeneric<T>>();
            bucketList.list.Add(entityData);
            ++this.count;
            this.buckets = buckets;

        }

        /// <summary>
        /// Get an element by Equals check and hashcode
        /// </summary>
        /// <param name="hashcode"></param>
        /// <param name="element"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public readonly bool Get(int hashcode, T element, out T output) {

            var bucket = (hashcode & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == true) {

                ref readonly var bucketList = ref bucketEntity.Read<IntrusiveHashSetBucketGeneric<T>>();
                foreach (var item in bucketList.list) {

                    if (element.Equals(item) == true) {

                        output = item;
                        return true;

                    }

                }

            }

            output = default;
            return false;

        }

        #region Helpers
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void Initialize(ref IntrusiveHashSetGeneric<T> hashSet) {

            if (hashSet.buckets.Length == 0) hashSet.buckets = new StackArray10<Entity>(10);

        }

        private static void InitializeComponents() {

            IntrusiveComponents.Initialize();
            WorldUtilities.InitComponentTypeId<IntrusiveHashSetBucketGeneric<T>>();
            ComponentInitializer.Init(ref Worlds.currentWorld.GetStructComponents());

        }

        private static class ComponentInitializer {

            public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {

                structComponentsContainer.Validate<IntrusiveHashSetBucketGeneric<T>>(false);

            }

        }
        #endregion

    }

}