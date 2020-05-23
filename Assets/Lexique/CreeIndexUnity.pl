use strict;
use warnings;
use List::Util qw(shuffle);
 
my $lexiqueFile = 'Lexique382.txt';
my $lexiqueAmnesiaFile = 'AmnesiaLexique.txt';
open(my $flexique, '<:encoding(UTF-8)', $lexiqueFile)
  or die "Could not open file '$lexiqueFile' $!";



my @index = (3,4,5,10,0);

my $row = <$flexique>;
chomp $row;

my %lexique =();


while ($row = <$flexique>) {
  chomp $row;
  my @table = split(/\t/, $row);
  my $isFirst = 1;
  if( $table[29] && $table[29] > 50){
    my $key = "";
    my $nature = $table[$index[0]];
    if ($nature eq "VER" ){
      foreach  my $conjugaison (split(/[;]/,$table[$index[3]])){
        $conjugaison =~ s/([:])/_/g;
        $key = "$table[$index[0]] $table[$index[1]]$table[$index[2]] $conjugaison";
        if(exists($lexique{$key})){
          my $liste = $lexique{$key};
          push(@$liste, $table[$index[4]]);
        }else{
          my @liste = ($table[$index[4]]);
          $lexique{$key} = \@liste;
        }
      }
    } elsif ($nature eq "NOM" || $nature eq "ADJ"){
      $key = "$table[$index[0]] $table[$index[1]]$table[$index[2]]";
        if(exists($lexique{$key})){
          my $liste = $lexique{$key};
          push(@$liste, $table[$index[4]]);
        }else{
          my @liste = ($table[$index[4]]);
          $lexique{$key} = \@liste;
        }
    } else {
      print "Nature inconue :$nature";
    }
  }
}

close $flexique;


my $directory = "mots";
unless(mkdir $directory) {
        die "Unable to create $directory\n";
}

open(my $index, '>encoding(UTF-8)', "$directory/index.txt")
    or die "Could not open file '$directory/index.txt' $!";

while( my ($k,$mot) = each(%lexique) ) {
  my $folder = "$directory/$k";
   unless(mkdir $folder) {
        die "Unable to create $folder\n";
  }
  my $taille = @$mot;
  @$mot = shuffle(@$mot);

  my $s = 0;
  while($taille > 0){
    open(my $famnesiaLexique, '>encoding(UTF-8)', "$folder/$s.txt")
    or die "Could not open file '$folder/$s.txt' $!";
    for my $i (0.. 99){
      if($taille>0){
        print $famnesiaLexique "".(pop @$mot)."\n";
      }
      $taille --;
    }
    $s ++;
    close $famnesiaLexique;
  }
   print $index "$k=$s\n";
}
close $index;

