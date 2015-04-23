using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace Website.Controllers
{
    public class Chat : XSocketController
    {
        /// ivory belongs to elephants...Just saying.
     
        private static readonly List<Animal> animals;
        /// <summary>
        /// Just a simple XSockets controller that illustrates a "backend" 
        /// </summary>
        public void GetAnimals()
        {
            //this.Send(animals,"getAnimals"); // Pass back the list of animals
        }

        /// <summary>
        /// Action that adds an "Animal" to our static list of animals. When added everyone is notified )
        /// </summary>
        /// <param name="animal"></param>
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
            //this.SendToAll(animal, "addAnimal"); // Notify all that we have a new animal
        }

        /// <summary>
        /// Remove an animal fro the list of Animals using it's Id ( Guid )
        /// </summary>
        /// <param name="id"></param>
        public void RemoveAnimal(Guid id)
        {
            var removedAnimal = animals.Find(a => a.Id.Equals(id));
            animals.Remove(removedAnimal);
            //this.SendToAll(removedAnimal, "removeAnimal"); // Notify all that an animal is removed...
        }

        static Chat()
        {
            animals = new List<Animal>(); // Just set up a animals
            animals.Add(new Animal() { Name = "Cat",Description = "Cats are also animals"});
            animals.Add(new Animal() { Name = "Dog", Description = "Dogs can bark..."});
            animals.Add(new Animal() { Name = "Bird" , Description = "Birds can fly"});

        }
    }

    /// <summary>
    ///  A model that describes our animals
    /// </summary>
    public class Animal
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public Animal()
        {
            this.Id = Guid.NewGuid();
        }
    }
}