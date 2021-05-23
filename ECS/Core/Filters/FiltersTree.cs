﻿#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct FiltersTree {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        private struct Item {

            public bool isCreated;
            public int bit;
            public int index;
            public ME.ECS.Collections.NativeBufferArray<int> filters;

            public void Dispose() {

                PoolArrayNative<int>.Recycle(ref this.filters);

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Add(FilterData filterData) {

                ArrayUtils.Resize(this.index, ref this.filters, true);
                var idx = this.filters.arr.IndexOf(filterData.id);
                if (idx == -1) {

                    this.filters.arr[this.index] = filterData.id;
                    ++this.index;

                }

            }

            public void CopyFrom(Item other) {

                this.isCreated = other.isCreated;
                this.bit = other.bit;
                this.index = other.index;
                ArrayUtils.Copy(other.filters, ref this.filters);

            }

            public void Recycle() {

                this.isCreated = default;
                this.bit = default;
                this.index = default;
                this.filters.Dispose();

            }

        }

        private struct CopyItem : IArrayElementCopy<Item> {

            public void Copy(Item @from, ref Item to) {

                to.CopyFrom(from);

            }

            public void Recycle(Item item) {

                item.Recycle();

            }

        }

        private ME.ECS.Collections.BufferArray<Item> itemsContains;
        private ME.ECS.Collections.BufferArray<Item> itemsNotContains;

        private ME.ECS.Collections.BufferArray<Item> itemsContainsVersioned;
        private ME.ECS.Collections.BufferArray<Item> itemsNotContainsVersioned;

        public void CopyFrom(FiltersTree other) {

            ArrayUtils.Copy(other.itemsContains, ref this.itemsContains, new CopyItem());
            ArrayUtils.Copy(other.itemsNotContains, ref this.itemsNotContains, new CopyItem());

            ArrayUtils.Copy(other.itemsContainsVersioned, ref this.itemsContainsVersioned, new CopyItem());
            ArrayUtils.Copy(other.itemsNotContainsVersioned, ref this.itemsNotContainsVersioned, new CopyItem());

        }

        public void Dispose() {

            for (var i = 0; i < this.itemsContains.Length; ++i) {

                this.itemsContains.arr[i].Dispose();

            }

            PoolArray<Item>.Recycle(ref this.itemsContains);

            for (var i = 0; i < this.itemsNotContains.Length; ++i) {

                this.itemsNotContains.arr[i].Dispose();

            }

            PoolArray<Item>.Recycle(ref this.itemsNotContains);

            for (var i = 0; i < this.itemsContainsVersioned.Length; ++i) {

                this.itemsContainsVersioned.arr[i].Dispose();

            }

            PoolArray<Item>.Recycle(ref this.itemsContainsVersioned);

            for (var i = 0; i < this.itemsNotContainsVersioned.Length; ++i) {

                this.itemsNotContainsVersioned.arr[i].Dispose();

            }

            PoolArray<Item>.Recycle(ref this.itemsNotContainsVersioned);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersContainsFor<T>() {

            var idx = WorldUtilities.GetComponentTypeId<T>();
            if (idx >= 0 && idx < this.itemsContains.Length) {

                return this.itemsContains.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersNotContainsFor<T>() {

            var idx = WorldUtilities.GetComponentTypeId<T>();
            if (idx >= 0 && idx < this.itemsNotContains.Length) {

                return this.itemsNotContains.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersContainsFor(int componentIndex) {

            var idx = componentIndex;
            if (idx >= 0 && idx < this.itemsContains.Length) {

                return this.itemsContains.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersNotContainsFor(int componentIndex) {

            var idx = componentIndex;
            if (idx >= 0 && idx < this.itemsNotContains.Length) {

                return this.itemsNotContains.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersContainsForVersioned<T>() {

            var idx = WorldUtilities.GetComponentTypeId<T>();
            if (idx >= 0 && idx < this.itemsContainsVersioned.Length) {

                return this.itemsContainsVersioned.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ME.ECS.Collections.NativeBufferArray<int> GetFiltersNotContainsForVersioned<T>() {

            var idx = WorldUtilities.GetComponentTypeId<T>();
            if (idx >= 0 && idx < this.itemsNotContainsVersioned.Length) {

                return this.itemsNotContainsVersioned.arr[idx].filters;

            }

            return ME.ECS.Collections.NativeBufferArray<int>.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(FilterData filter) {

            ref var contains = ref this.itemsContains;
            ref var notContains = ref this.itemsNotContains;

            if (filter.onVersionChangedOnly == true) {

                contains = ref this.itemsContainsVersioned;
                notContains = ref this.itemsNotContainsVersioned;

            }

            {

                var bits = filter.archetypeContains.BitsCount;
                for (var i = 0; i <= bits; ++i) {

                    if (filter.archetypeContains.HasBit(i) == true) {

                        ArrayUtils.Resize(i, ref contains, true);
                        ref var item = ref contains.arr[i];
                        if (item.isCreated == false) {
                            item = new Item() { isCreated = true, bit = i, index = 0, };
                        }

                        item.Add(filter);

                    }

                }

            }

            {

                var bits = filter.archetypeNotContains.BitsCount;
                for (var i = 0; i <= bits; ++i) {

                    if (filter.archetypeNotContains.HasBit(i) == true) {

                        ArrayUtils.Resize(i, ref notContains, true);
                        ref var item = ref notContains.arr[i];
                        if (item.isCreated == false) {
                            item = new Item() { isCreated = true, bit = i, index = 0, };
                        }

                        item.Add(filter);

                    }

                }

            }

        }

    }

}