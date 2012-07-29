using System;
using System.Collections.Generic;
using System.Text;

namespace Cards {
	/// <summary>
	/// Class that represents a single card.  This is a true business class; 
	/// it represents everything about a card that we think is important.
	/// Also, note that it supports features we don't actually need for Blackjack, 
	/// such as the Joker.
	/// </summary>
	public class Card {
		/// <summary>
		/// Flipped event, fired when the card's face up/down setting is actually changed.
		/// </summary>
		public event EventHandler Flipped;

		/// <summary>
		/// Defaults (this makes them easy to change everywhere at once):
		/// </summary>
		public const Backs DefaultBack = Backs.Bellagio;

		/// <summary>
		/// Face-up property, with code to fire Flipped event.
		/// </summary>
		private bool faceUp = false;
	

		// Simple properties:
		public Suits Suit { 
            get; 
            private set; 
        }

		public Ranks Rank { 
            get; 
            private set; 
        }

		public Backs Back { 
            get; 
            private set; 
        }

	    public bool FaceUp {
			get { 
                return faceUp; 
            }
			set {
				bool oldValue = faceUp;
				faceUp = value;
                if (oldValue != faceUp && Flipped != null) {
                    Flipped(this, EventArgs.Empty);
                }
			}
		}

		/// <summary>
		/// Card's name (e.g. "Jack of Spades")
		/// </summary>
		public string Name {
			get {
				if (Rank == Ranks.Joker) return Rank.ToString();
				return Rank + " of " + Suit; 
			}
		}

		/// <summary>
		/// Card's display value (card name if face-up, card back if face-down).
		/// </summary>
		public string CardDisplay {
			get {
                if (FaceUp) {
                    return Name;
                }

				return Back.ToString();
			}
		}

		public Card(Suits suit, Ranks rank) 
            :this(suit, rank, DefaultBack) {	
        }

		public Card(Suits suit, Ranks rank, Backs back) {
			Suit = (rank == Ranks.Joker ? Suits.None : suit);
			Rank = rank;
			Back = back;
		}

		public override string ToString() {
			return CardDisplay;
		}
	}
}
