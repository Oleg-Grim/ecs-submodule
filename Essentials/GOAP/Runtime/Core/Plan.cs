namespace ME.ECS.Essentials.GOAP {

    using Collections;
    
    public enum PathStatus {

        NotCalculated,
        Processing,
        Failed,
        Success,

    }

    public struct Plan {

        public PathStatus planStatus;
        public float cost;
        public BufferArray<Action> actions;

        public void Dispose() {

            this.planStatus = PathStatus.NotCalculated;
            PoolArray<Action>.Recycle(ref this.actions);
            
        }

        public override string ToString() {

            var str = "Plan ";
            var cost = 0f;
            foreach (var action in this.actions) {
                cost += action.data.cost;
            }

            str += " Cost(" + cost + "): ";

            foreach (var action in this.actions) {
                str += " => [" + action.data.groupId + "]";
            }

            return str;

        }

    }

}