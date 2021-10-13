﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1
{

    class Context
    {
        State current_state;

        public Context(State current_state)
        {
            this.transitionToState(current_state);
        }

        public void transitionToState(State state_to_set)
        {
            current_state = state_to_set;
            current_state.Context = this;
        }

        private void draw()  // todo make private
        {
            this.current_state.draw();
            Console.WriteLine("type EXIT to exit");
        }

        private void handle(string user_input)
        {
            Console.WriteLine("Handle your input...");
            this.current_state.handle(user_input);
        }

        public void run() 
        {
            while (true)
            {
                this.draw();
                string user_input = Console.ReadLine();
                if (user_input == "EXIT") break;
                this.handle(user_input);
            }
        }
    }


    abstract class State
    {
        protected string title;
        protected const string title_frame = "======"; // string to be drawn from both sides of the title
        protected string state_description;
        protected List<State> children = new List<State>(); // states that are accessible from this state 
        public State Parent { get; set; } = null; // state that can lead to this state
        public Context Context { get; set; }

        public void become_state_parent(State child)
        {
            this.children.Add(child);
            child.Parent = this;
        }

        public void handle_navigation(string user_input)
        {
            foreach (State child in this.children)
            {
                if (user_input == child.title)
                {
                    Context.transitionToState(child);
                }
            }

            if (user_input == "BACK" && this.Parent != null)
            {
                this.Context.transitionToState(this.Parent);
            }
        }

        public void draw_navigation_guide()
        {
            Console.WriteLine(State.title_frame + this.title + State.title_frame);
            foreach (State child in this.children)
            {
                Console.WriteLine("type " + child.title + " to " + child.state_description);
            }
            if (this.Parent != null)
            {
                Console.WriteLine("type BACK to return to the previous state");
            }
        }

        public abstract void handle(string user_input);
        public abstract void draw();
    }


    class ConcreteState : State
    {
        public ConcreteState(string title, string description)
        {
            this.title = title;
            this.state_description = description;
        }

        public override void handle(string user_input)
        {
            this.handle_navigation(user_input);
        }

        public override void draw()
        {
            this.draw_navigation_guide();
        }
    }


    abstract class MusContent
    {
        public string Name { get; protected set; }
        public abstract void show_info();
    }


    class Album : MusContent
    {
        private List<Song> songs;
        public DateTime Release_date { get; private set; }
        public Genre Album_genre { get; private set; }
        public Performer Album_performer { get; set; }
        private const string song_enumeration_indent = "    ";

        public Album(string name, Genre album_genre, DateTime release_date)
        {
            this.Name = name;
            this.songs = new List<Song>();
            this.Album_genre = album_genre;
            this.Release_date = release_date;
        }

        public void add_song_to_album(Song song)
        {
            this.songs.Add(song);
            song.Song_album = this;
        }
        public override void show_info()
        {
            Console.WriteLine("Album: " + this.Name);
            Console.WriteLine("Release date: " + this.Release_date.ToShortDateString());
            Console.WriteLine("Genre: ", this.Album_genre.Name);
            if (this.Album_performer == null)
            {
                Console.WriteLine("No information about the performer yet");
            }
            else
            {
                Console.WriteLine("Performed by: " + this.Album_performer.Name);
            }
            if (this.songs.Count > 0)
            {
                Console.WriteLine("Songs list:");
                foreach (Song song in songs)
                {
                    Console.WriteLine(Album.song_enumeration_indent + song.Name);
                }
            }
            else
            {
                Console.WriteLine("No information about the songs yet");
            }
        }

        public void add_song()
        {

        }
    }


    class Performer : MusContent
    {
        public DateTime Formation_date { get; private set; }
        public Genre Performer_genre { get; private set; }
        private List<Album> albums;
        private List<Song> songs;

        public override void show_info()
        {
            this.albums = new List<Album>();
            this.songs = new List<Song>();
        }

        public void register_song()
        {

        }

        public void register_album()
        {

        }
    }


    class Song : MusContent
    {
        public DateTime Release_date { get; private set; }
        public Genre Mus_genre { get; private set; }
        public Performer Song_performer { get; set; }
        public Album Song_album { get; set; }

        public Song(string name, Genre mus_genre, DateTime release_date)
        {
            this.Name = name;
            this.Mus_genre = mus_genre;
            this.Release_date = release_date;
        }

        public override void show_info()
        {
            string description = "Song: " + this.Name; 
        }
    }


    class Genre : MusContent
    {
        public Genre(string name)
        {
            this.Name = name;
        }

        public override void show_info()
        {
            Console.WriteLine("Genre: " + this.Name);
        }
    }


    class Library
    {
        private List<MusContent> content;

        public Library()
        {
            content = new List<MusContent>();
        }

        public void add_content(MusContent content)
        {
            this.content.Add(content);
        }

        public List<MusContent> get_news()
        {
            List<MusContent> res = new List<MusContent>();
            return res;
        }

        //public MusContent search()
        //{
        //    MusContent res = new Song(@@);
        //    return res;
        //}
    }


    class Program
    {

        static Context build_app()
        {
            State mainMenu = new ConcreteState("MainMenu", "MainMenu");
            ConcreteState news = new ConcreteState("NEWS", "see the latest news");
            ConcreteState search = new ConcreteState("SEARCH", "search for specific things");
            mainMenu.become_state_parent(news);
            mainMenu.become_state_parent(search);
            Context app = new Context(mainMenu);
            return app;
        }

        static void Main(string[] args)
        {
            DateTime test = new DateTime(1, 1, 1);
            Console.WriteLine("string: " + test.ToShortDateString());
            //titile parent son
            Context app = build_app();
            //app.run();
        }
    }
}
