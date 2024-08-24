using TelegramBotFramework.Commands.Utils.Mappers;
using Xunit;

namespace TelegramBotFramework.Tests
{
    public enum Age {
        Small, Normik, EbatStariy
    }

    public record WarriorContext(string Name, Age Age);

    public class MapperTests {
        [Fact]
        public void Test() {
            var dictionary = new Dictionary<Type, TypeMappingDescriptor> {
                { typeof(WarriorContext), CreateWarriorDescriptor() }
            };

            var config = new MapperConfiguration(dictionary);
            var mapper = new Mapper(config);
            var context = mapper.Map(typeof(WarriorContext), ["EXREGISTR", "228"]);
            Assert.NotNull(context);
        }

        private static TypeMappingDescriptor CreateDescriptor<T>() {
            var type = typeof(T);
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();

            var propertyMappings = parameters
                .Select(property => {
                    var descriptor = new PropertyMappingDescriptor(value => {
                        return Convert.ChangeType(value, property.ParameterType);
                    });
                    return descriptor;
                });

            var descriptor = new TypeMappingDescriptor(type, constructor, propertyMappings.ToArray());
            return descriptor;
        }

        private static TypeMappingDescriptor CreateWarriorDescriptor() {
            var type = typeof(WarriorContext);
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();

            var propertyMappings = new PropertyMappingDescriptor[] {
                new (name => name),
                new (ageInput => {
                    var age = int.Parse(ageInput);
                    if (age < 15) {
                        return Age.Small;
                    }

                    if (age < 50) {
                        return Age.Normik;
                    }

                    return Age.EbatStariy;
                })
            };

            var descriptor = new TypeMappingDescriptor(type, constructor, propertyMappings);
            return descriptor;
        }
    }
}
