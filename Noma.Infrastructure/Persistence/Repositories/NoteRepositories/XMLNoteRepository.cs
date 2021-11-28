using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Noma.Infrastructure.Persistence.Repositories.NoteRepositories
{
    public class XMLNoteRepository : INoteRepository
    {
        const string xmlFileDirectory = @"C:\Users\simas\Desktop";
        const string xmlFileName = "notes.xml";
        const string xmlFilePath = xmlFileDirectory + "\\" + xmlFileName;

        public XMLNoteRepository()
        {
            LoadNoteXML();
        }

        NoteXML noteXML = new NoteXML();

        private void LoadNoteXML()
        {
            if (!File.Exists(xmlFilePath))
                CreateEmptyXMLDatabase();

            var serializer = new XmlSerializer(typeof(NoteXML));
            using (var reader = XmlReader.Create(xmlFilePath))
            {
                noteXML = (NoteXML)serializer.Deserialize(reader);
            }
        }

        private void CreateEmptyXMLDatabase()
        {
            NoteXML newNoteXML = new NoteXML();
            var serializer = new XmlSerializer(typeof(NoteXML));
            using (var writer = XmlWriter.Create(xmlFilePath))
            {
                serializer.Serialize(writer, newNoteXML);
            }
        }

        public Task Save()
        {
            var serializer = new XmlSerializer(typeof(NoteXML));
            using (var writer = XmlWriter.Create(xmlFilePath))
            {
                serializer.Serialize(writer, noteXML);
            }

            return Task.CompletedTask;
        }

        public Task<Note> DeleteNote(int id)
        {
            var note = noteXML.Notes.FirstOrDefault(n => n.Id == id);

            noteXML.Notes.Remove(note);
            return Task.FromResult(note);
        }

        public Task<Note> GetNoteById(int id)
        {
            return Task.FromResult(noteXML.Notes.ToList().FirstOrDefault(n => n.Id == id));
        }

        public Task<IEnumerable<Note>> GetNotes()
        {
            return Task.FromResult(noteXML.Notes.ToList().AsEnumerable());
        }

        public Task<Note> InsertNote(Note note)
        {
            Note n = new Note()
            {
                Id = 1,
                Title = note.Title,
                Content = note.Content,
                InTrash = note.InTrash
            };

            noteXML.Notes.Add(n);

            return Task.FromResult(n);
        }

        public Task<Note> UpdateNote(Note note)
        {
            Note temp = null;
            if(noteXML.Notes.FirstOrDefault(_note => _note.Id == note.Id) is Note n)
            {
                temp = n;
                n.Title = note.Title;
                n.Content = note.Content;
                n.InTrash = note.InTrash;
            }

            return Task.FromResult(temp);
        }
    }
}
