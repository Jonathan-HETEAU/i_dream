use strict;
use locale;
use warnings;
use Data::Dumper;



sub indexer{
    my ($value,$refIndice, $refIndex, $refDico) = @_;
    my %dico = %$refDico;
    if(exists( $dico{$value}) ){
        $refIndex->{$value}= $$refIndice ;
        $$refIndice ++;
        return "\n$dico{$value}\|$refIndex->{$value}\n";
       
    }
    return "$value";
}

sub remplacer{
    my ($value,$refDico) = @_;
    my %dico = %$refDico;
    if(exists( $dico{$value}) ){
      return "\n$dico{$value}\n";
    }
    return "$value";
}

sub referencer{
    my ($value, $refIndex) = @_;
    my %index = %$refIndex;
    if(exists( $index{$value}) ){
        return "\{$index{$value}\}";
    }
    return "$value";
}

sub generationDico{
    my $lexiqueFile = 'Lexique382.txt';
    open(my $flexique, '<:encoding(UTF-8)', $lexiqueFile)
        or die "Could not open file '$lexiqueFile' $!";

    my @index = (3,4,5,10,0);

    my $row = <$flexique>;
    chomp $row;

    my %dico =();


    while ($row = <$flexique>) {
    chomp $row;
    my @table = split(/\t/, $row);
    my $isFirst = 1;
        my $key = "";
        my $nature = $table[$index[0]];
        if ($nature eq "VER" ){
        my @conjugaison = (split(/[;]/,$table[$index[3]]));
        if( @conjugaison == 1){
            $conjugaison[0] =~ s/([:])/_/g;
            $key = "$table[$index[0]] $table[$index[1]]$table[$index[2]] $conjugaison[0]";
            $dico{$table[$index[4]]} = $key;
        }
        
        } elsif ($nature eq "NOM" || $nature eq "ADJ"){
        $key = "$table[$index[0]] $table[$index[1]]$table[$index[2]]";
            $dico{$table[$index[4]]} = $key;
        } else {
        
        }
    }

    close $flexique;
    return %dico;
}


my %dico = generationDico();

my $texte = "rever.txt";
open(my $fTexte, '<:encoding(UTF-8)', $texte)
  or die "Could not open file '$texte' $!";

open(my $reve, '>encoding(UTF-8)', "reve.unity.txt")
    or die "Could not open file 'reve.unity.txt' $!";

my %index = ();
my $indice = 0;
while ( my $row = <$fTexte>) {
    chomp $row;
    $row =~ s/\[([^\]]*)\]/indexer($1,\$indice,\%index, \%dico)/ge;
    $row =~ s/\(([^\)]*)\)/remplacer($1,\%dico)/ge;
    print "@{[%index]}";
    $row =~ s/\{([^\}]*)\}/referencer($1,\%index)/ge;
    print $reve $row ;
}
close $reve;
close $fTexte;
