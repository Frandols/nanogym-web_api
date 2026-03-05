using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Gym
    {
        public Guid Id { get; private set; }
        public Guid OwnerId { get; private set; }
        private readonly List<Administrator> _administrators;
        public IReadOnlyCollection<Administrator> Administrators => _administrators.AsReadOnly();
        public List<Plan> _plans;
        public IReadOnlyCollection<Plan> Plans => _plans.AsReadOnly();
        public string Name { get; private set; } = string.Empty;
        public Location Location { get; private set; }

        private Gym()
        {
            _administrators = [];
            _plans = [];
            Location = null!;
        }

        public Gym(CreateGymInput input) 
        {
            ValidateName(input.Name);
            ValidateLocation(input.Location);

            Id = Guid.NewGuid();
            OwnerId = input.OwnerId;
            _administrators = [];
            _plans = [];
            Name = input.Name;
            Location = input.Location;
        }

        static private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Gym name cannot be null or empty.", nameof(name));
        }

        static private void ValidateLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location), "Location cannot be null.");
        }

        public void Update(UpdateGymInput input)
        {
            ValidateName(input.Name);
            ValidateLocation(input.Location);

            Name = input.Name;
            Location = input.Location;
        }

        public InvitationToAdministrate GenerateInvitationToAdministrate()
        {
            var createInvitationToAdministrateInput = new CreateInvitationToAdministrateInput(
                GymId: Id, 
                ValidFor: TimeSpan.FromDays(7));

            var invitationToAdministrate = new InvitationToAdministrate(createInvitationToAdministrateInput);

            return invitationToAdministrate;
        }

        public void AddAdministrator(Guid userId)
        {
            if (_administrators.Any(administrator => administrator.UserId == userId))
                throw new InvalidOperationException("User is already an administrator of this gym.");

            if(userId == OwnerId)
                throw new InvalidOperationException("Owner cannot be added as an administrator.");

            var administrator = new Administrator(userId);

            _administrators.Add(administrator);
        }

        public void AddPlan(CreatePlanInput input)
        {
            if (_plans.Any(existingPlan => existingPlan.Name == input.Name)) 
                throw new InvalidOperationException("Cannot have two plans with the same exact name");

            var createPlanInput = new CreatePlanInput(
                Name: input.Name,
                Price: input.Price,
                Duration: input.Duration);

            var plan = new Plan(createPlanInput);

            _plans.Add(plan);
        }

        public Plan GetPlanWithId(Guid planId)
        {
            var plan = _plans.FirstOrDefault(p => p.Id == planId);

            if (plan == null)
                throw new InvalidOperationException("Plan not found.");
            
            return plan;
        }

        public void UpdatePlanWithId(Guid planId, UpdatePlanInput input)
        {
            var plan = _plans.FirstOrDefault(p => p.Id == planId);

            if (plan == null)
                throw new InvalidOperationException("Plan not found.");

            if(_plans.Any(existingPlan => existingPlan.Name == input.Name && existingPlan.Id != planId)) 
                throw new InvalidOperationException("Cannot have two plans with the same exact name");

            plan.Update(input);
        }
    }

    public sealed record CreateGymInput(
        Guid OwnerId,
        string Name,
        Location Location);

    public sealed record UpdateGymInput(
        string Name,
        Location Location);
}
